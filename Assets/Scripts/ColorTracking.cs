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
    //private bool hayMovimiento = false;
    // Variable para almacenar el estado del movimiento de color rojo
    private bool hayMovimientoRojo = false;
    // Variable para almacenar el estado del movimiento de color verde
    private bool hayMovimientoVerde = false;
    // Variable para almacenar el fotograma anterior
    private Mat prevFrame = null;
    // Variable para almacenar el umbral de diferencia
    //private double threshold = 5;
    private double threshold_red = 6; // Variable para almacenar el umbral de diferencia para objetos de color rojo
    private double threshold_green = 4; // Variable para almacenar el umbral de diferencia para objetos de color verde
    // Variable para almacenar el número de objetos de color rojo
    //private int num_objects_red = 0;
    // Variable para almacenar el número de objetos de color verde
    //private int num_objects_green = 0;

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

        // Convertir el fotograma a HSV
        Mat hsv = new Mat();
        Cv2.CvtColor(frame, hsv, ColorConversionCodes.BGR2HSV);

        // Definir el rango de color rojo en HSV
        //Scalar lower_red = new Scalar(0, 120, 70);    //Valor del tono para un rojo amplio
        //Scalar upper_red = new Scalar(10, 255, 255);

        Scalar lower_red = new Scalar(0, 100, 99); //Valor del tono para un rojo específico
        Scalar upper_red = new Scalar(0, 100, 100);

        // Definir el rango de color verde en HSV
        Scalar lower_green = new Scalar(36, 25, 25); //Valor del tono para un verde amplio
        Scalar upper_green = new Scalar(86, 255, 255);

        //Scalar lower_green = new Scalar(220, 100, 100);  // Valor del tono para azul
        //Scalar upper_green = new Scalar(240, 100, 100);

        //Scalar lower_green = new Scalar(120, 100, 99); //Valor del tono para un verde específico
        //Scalar upper_green = new Scalar(120, 100, 100);

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


        //Analizar ROI (Region de Interes) de objetos de color verde
        foreach (var contour in contours_green)
        {
            //Obtener una ROI para cada contorno de cada objeto verde
            OpenCvSharp.Rect roi_green = Cv2.BoundingRect(contour);

            //Extraer la ROI de la mascara de movimiento
            Mat roi_Mask_Green = new Mat(closed_green, roi_green);

            //Calcular el valor de la media de movimientod e cada objeto
            Scalar roiMeanGreen = Cv2.Mean(roi_Mask_Green);


            if (roiMeanGreen.Val0 > threshold_green)
            {
                // Definir el texto del mensaje
                string message = $"Se han detectado objetos de color VERDE que se mueven mucho";
                Debug.Log("El valor del meanGreen: " + roiMeanGreen.Val0);
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
                // Definir el color de los contornos verdes
                Scalar contour_color_green = Scalar.Green;
                // Definir el grosor de los contornos
                int contour_thickness = 2;
                // Dibujar los contornos en el fotograma original
                Cv2.DrawContours(frame, contours_green, -1, contour_color_green, contour_thickness);

                hayMovimientoVerde = true;
                
            }
            else
            {
                hayMovimientoVerde = false;
            }
        }

        //Analizar ROI (Region de Interes) de objetos de color rojo
        foreach (var contour in contours_red)
        {
            //Obtener una ROI para cada contorno de cada objeto rojo
            OpenCvSharp.Rect roi_red = Cv2.BoundingRect(contour);

            //Extraer la ROI de la mascara de movimiento
            Mat roi_Mask_Red = new Mat(closed_red, roi_red);

            //Calcular el valor de la media de movimientod e cada objeto
            Scalar roiMeanRed = Cv2.Mean(roi_Mask_Red);

            if (roiMeanRed.Val0 > threshold_red)
            {
                // Definir el texto del mensaje
                string message = $"Se han detectado objetos de color ROJO que se mueven mucho";
                Debug.Log("El valor del meanRed: " + roiMeanRed.Val0);
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
                // Definir el grosor de los contornos
                int contour_thickness = 2;
                // Dibujar los contornos en el fotograma original
                Cv2.DrawContours(frame, contours_red, -1, contour_color_red, contour_thickness);

                hayMovimientoRojo = true;
                
                //return frame;
            }
            else
            {
                hayMovimientoRojo = false;
            }
        }


        Thread.Sleep(1000 / 60);
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
