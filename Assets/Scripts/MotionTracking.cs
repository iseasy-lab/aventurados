using UnityEngine;
using System;
using OpenCvSharp;

//**********************************
//Script para detectar el movimiento
//**********************************

public class MotionTracking : MonoBehaviour
{
    // Variable para almacenar el video
    private WebCamTexture webcamTexture;
    // Variable para almacenar el fondo
    private BackgroundSubtractorMOG2 backgroundSubtractor;
    // Variable para almacenar el kernel morfológico
    private Mat kernel;

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

    void Update()
    {
        // Obtener el fotograma actual
        Mat frame = OpenCvSharp.Unity.TextureToMat(webcamTexture);
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
        // Dibujar un rectángulo alrededor de cada contorno
        foreach (Point[] contour in contours)
        {
            OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
            Cv2.Rectangle(frame, rect, Scalar.Red, 2);
        }
        // Mostrar el resultado
        Cv2.ImShow("Motion Detection", frame);
    }

    void OnDestroy()
    {
        // Liberar los recursos
        webcamTexture.Stop();
        backgroundSubtractor.Dispose();
        kernel.Dispose();
        Cv2.DestroyAllWindows();
    }
}
