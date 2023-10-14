using UnityEngine;
using System;
using OpenCvSharp;
using System.Threading.Tasks;

//**********************************
//Script para detectar el color rojo
//**********************************

public class ColorTracking : MonoBehaviour
{
    // Variable para almacenar el video
    private WebCamTexture webcamTexture;
    // Variable para almacenar el fondo
    private BackgroundSubtractorMOG2 backgroundSubtractor;
    // Variable para almacenar el kernel morfológico
    private Mat kernel;
    // Variable para almacenar el estado del movimiento
    private bool hayMovimiento = false;
    // Variable para almacenar el fotograma anterior
    private Mat prevFrame = null;
    // Variable para almacenar el umbral de diferencia
    //private double threshold = 4.5;
    private double threshold = 5;

    void Start()
    {
        // Inicializar el video
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();
        // Inicializar el fondo
        backgroundSubtractor = BackgroundSubtractorMOG2.Create();
        // Inicializar el kernel morfológico
        kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
    }

    async void Update()
    {
        // Obtener el fotograma actual de forma asíncrona
        Mat frame = await GetFrameAsync();
        // Mostrar el resultado si hay movimiento
        if (hayMovimiento)
        {
            Cv2.ImShow("Red Color Detection", frame);
            Debug.Log("Se ha detectado un objeto de color rojo que se mueve");
            // Aquí se puede agregar más código para hacer algo con el objeto detectado
        }

    }

    async Task<Mat> GetFrameAsync()
    {
        // Obtener el fotograma actual
        Mat frame = OpenCvSharp.Unity.TextureToMat(webcamTexture);
        // Si es el primer fotograma, asignarlo al previo y salir del método
        if (prevFrame == null)
        {
            prevFrame = frame.Clone();
            return frame;
        }
        // Calcular la diferencia entre el fotograma actual y el previo
        Mat diff = new Mat();
        Cv2.Absdiff(frame, prevFrame, diff);
        // Calcular la suma de los valores absolutos de la diferencia
        Scalar sum = Cv2.Sum(diff);
        // Calcular la media de los valores absolutos de la diferencia
        double mean = (sum[0] + sum[1] + sum[2]) / (diff.Rows * diff.Cols * diff.Channels());
        // Actualizar el fotograma previo con el actual
        prevFrame = frame.Clone();
        // Si la media es menor que el umbral, asignar false a la variable booleana y salir del método
        if (mean < threshold)
        {
            hayMovimiento = false;
            return frame;
        }

        // Convertir el fotograma a escala de grises
        Mat gray = new Mat();
        Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
        // Aplicar la sustracción de fondo
        Mat fgMask = new Mat();
        backgroundSubtractor.Apply(gray, fgMask);
        // Aplicar la operación morfológica de cierre
        Mat closed = new Mat();
        Cv2.MorphologyEx(fgMask, closed, MorphTypes.Close, kernel);
        // Encontrar los contornos de los objetos en movimiento
        Point[][] contours;
        HierarchyIndex[] hierarchy;
        Cv2.FindContours(closed, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
        // Contar el número de contornos encontrados
        int num_objects = contours.Length;

        // Si hay al menos un contorno, mostrar un mensaje y dibujar los contornos
        if (num_objects > 0)
        {
            // Definir el texto del mensaje
            string message = $"Se han detectado {num_objects} objetos de color rojo.";
            // Definir la posición del mensaje
            Point position = new Point(10, 30);
            // Definir el color del mensaje
            Scalar color = Scalar.White;
            // Definir la fuente del mensaje
            HersheyFonts font = HersheyFonts.HersheySimplex;
            // Definir el tamaño del mensaje
            double size = 1.0;
            // Escribir el mensaje en el fotograma original
            Cv2.PutText(frame, message, position, font, size, color);
            // Definir el color de los contornos
            Scalar contour_color = Scalar.Blue;
            // Definir el grosor de los contornos
            int contour_thickness = 2;
            // Dibujar los contornos en el fotograma original
            Cv2.DrawContours(frame, contours, -1, contour_color, contour_thickness);
            // Asignar el valor true a la variable booleana
            hayMovimiento = true;
        }

        // Mostrar el fotograma con la diferencia resaltada en una ventana aparte
        Cv2.ImShow("Frame Difference", diff);

        return frame;
    }

    void OnDestroy()
    {
        // Liberar los recursos
        webcamTexture.Stop();
        backgroundSubtractor.Dispose();
        kernel.Dispose();
        prevFrame.Dispose();
        Cv2.DestroyAllWindows();
    }
}
