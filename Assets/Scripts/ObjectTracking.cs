using UnityEngine;
using System;
using OpenCvSharp;

public class ObjectTracking : MonoBehaviour
{
    // Variable para almacenar el video
    private WebCamTexture webcamTexture;
    // Variable para almacenar el rango de color rojo
    private Scalar lowerRed;
    private Scalar upperRed;
    // Variable para almacenar el kernel morfológico
    private Mat kernel;

    void Start()
    {
        // Inicializar el video
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();
        // Inicializar el rango de color rojo
        lowerRed = new Scalar(0, 100, 100);
        upperRed = new Scalar(10, 255, 255);
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
        // Aplicar el filtro de color
        Mat mask = new Mat();
        Cv2.InRange(gray, lowerRed, upperRed, mask);
        // Aplicar la operación morfológica de cierre
        Mat closed = new Mat();
        Cv2.MorphologyEx(mask, closed, MorphTypes.Close, kernel);
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
        Cv2.ImShow("Color Tracking", frame);
    }

    //void OnDestroy()
    //{
        //// Liberar los recursos
        //webcamTexture.Stop();
       // lowerRed.Dispose();
        //upperRed.Dispose();
        //kernel.Dispose();
        //Cv2.DestroyAllWindows();
    //}

}
