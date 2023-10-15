using UnityEngine;
using System;
using OpenCvSharp;
using System.Threading.Tasks;
using System.Threading;

//**********************************
//Script para detectar el color rojo y el verde
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
    // Variable para almacenar el estado del movimiento de color rojo
    private bool hayMovimientoRojo = false;
    // Variable para almacenar el estado del movimiento de color verde
    private bool hayMovimientoVerde = false;
    // Variable para almacenar el fotograma anterior
    private Mat prevFrame = null;
    // Variable para almacenar el umbral de diferencia
    private double threshold = 5;
    // Variable para almacenar el número de objetos de color rojo
    private int num_objects_red = 0;
    // Variable para almacenar el número de objetos de color verde
    private int num_objects_green = 0;

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
        // Obtener el número de fotogramas desde el inicio
        int frameCount = Time.frameCount;
        // Definir el divisor para reducir la frecuencia de análisis
        int divisor = 4;
        // Comprobar si el fotograma actual es divisible por el divisor
        if (frameCount % divisor == 0)
        {
            // Obtener el fotograma actual de forma asíncrona
            Mat frame = await GetFrameAsync();
            // Mostrar el resultado solo si hay movimiento y hay movimiento de color rojo o verde
            if (hayMovimientoRojo || hayMovimientoVerde)
            //if (hayMovimiento && (hayMovimientoRojo || hayMovimientoVerde))
            {
                Cv2.ImShow("Color Detection", frame);
                Debug.Log("Se ha detectado un objeto de color rojo o verde que se mueve");
                // Aquí se puede agregar más código para hacer algo con el objeto detectado
            }
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
            hayMovimientoRojo = false;
            hayMovimientoVerde = false;
            num_objects_red = 0;
            num_objects_green = 0;
            Debug.Log("No hay objetos");
            Thread.Sleep(1000); //duerme el programa por 1s
            return null;
        }

        // Convertir el fotograma a HSV
        Mat hsv = new Mat();
        Cv2.CvtColor(frame, hsv, ColorConversionCodes.BGR2HSV);

        // Definir el rango de color rojo en HSV
        Scalar lower_red = new Scalar(0, 120, 70);
        Scalar upper_red = new Scalar(10, 255, 255);

        // Definir el rango de color verde en HSV
        Scalar lower_green = new Scalar(36, 25, 25);
        Scalar upper_green = new Scalar(86, 255, 255);

        // Crear una máscara binaria para el color rojo y otra para el color verde
        Mat red_mask = new Mat();
        Mat green_mask = new Mat();
        Cv2.InRange(hsv, lower_red, upper_red, red_mask);
        Cv2.InRange(hsv, lower_green, upper_green, green_mask);

        // Aplicar la operación morfológica de cierre a cada máscara
        Mat closed_red = new Mat();
        Mat closed_green = new Mat();
        Cv2.MorphologyEx(red_mask, closed_red, MorphTypes.Close, kernel);
        Cv2.MorphologyEx(green_mask, closed_green, MorphTypes.Close, kernel);

        // Encontrar los contornos de los objetos rojos y verdes usando las máscaras correspondientes
        Point[][] contours_red;
        Point[][] contours_green;
        HierarchyIndex[] hierarchy_red;
        HierarchyIndex[] hierarchy_green;
        Cv2.FindContours(closed_red, out contours_red, out hierarchy_red, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
        Cv2.FindContours(closed_green, out contours_green, out hierarchy_green, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

        // Contar el número de contornos encontrados y asignarlo a las variables
        num_objects_red = contours_red.Length;
        num_objects_green = contours_green.Length;

        // Asignar el valor true a las variables booleanas si hay al menos un contorno de cada color, y false en caso contrario
        hayMovimientoRojo = num_objects_red > 0;
        hayMovimientoVerde = num_objects_green > 0;

        // Si hay al menos un contorno de cada color, mostrar un mensaje y dibujar los contornos
        if (hayMovimientoRojo || hayMovimientoVerde)
        {
            // Definir el texto del mensaje
            string message = $"Se han detectado {num_objects_red} objetos de color rojo y {num_objects_green} objetos de color verde.";
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
            // Definir el color de los contornos rojos
            Scalar contour_color_red = Scalar.Red;
            // Definir el color de los contornos verdes
            Scalar contour_color_green = Scalar.Green;
            // Definir el grosor de los contornos
            int contour_thickness = 2;
            // Dibujar los contornos en el fotograma original
            Cv2.DrawContours(frame, contours_red, -1, contour_color_red, contour_thickness);
            Cv2.DrawContours(frame, contours_green, -1, contour_color_green, contour_thickness);
            // Asignar el valor true a la variable booleana
            Thread.Sleep(1000);
            hayMovimiento = true;
        }

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
