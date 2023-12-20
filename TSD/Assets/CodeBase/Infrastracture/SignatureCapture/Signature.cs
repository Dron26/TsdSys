using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastracture.SignatureCapture
{
    public class Signature : MonoBehaviour
    {
        [SerializeField] private Image imageCanvas;
        [SerializeField] private float brushSize = 10f;
        [SerializeField] private Color brushColor = Color.black;
        [SerializeField] private int smoothness = 20; // Количество точек между текущей и предыдущей позициями
        [SerializeField] private Vector2? previousPosition = null;
        [SerializeField] private Button clearButton;
        [SerializeField] private Texture2D drawingTexture;
        [SerializeField] private RectTransform imageRect;


        [SerializeField] private InputField sizeInput; // Поле ввода для размера кисти
        [SerializeField] private InputField smoothnessInput;

        void Start()
        {
            InitializeTexture();
            clearButton.onClick.AddListener(ClearDrawing);
            sizeInput.onValueChanged.AddListener(UpdateBrushSize); // Обновление размера кисти
            smoothnessInput.onValueChanged.AddListener(UpdateSmoothness); // Обновление плавности
        }

        void UpdateBrushSize(string size)
        {
            float.TryParse(size, out brushSize); // Преобразование строки в число
        }

        void UpdateSmoothness(string smooth)
        {
            int.TryParse(smooth, out smoothness); // Преобразование строки в число
        }

        void ClearDrawing()
        {
            ClearTexture(); // Очистка текстуры
            ApplyTextureToImage(); // Применение изменений на изображение
        }

        void InitializeTexture()
        {
            imageRect = imageCanvas.rectTransform;
            drawingTexture = new Texture2D((int)imageRect.rect.width, (int)imageRect.rect.height);
            ClearTexture();
            ApplyTextureToImage();
        }

        void ClearTexture()
        {
            Color[] fillColorArray = drawingTexture.GetPixels();

            for (int i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.clear;
            }

            drawingTexture.SetPixels(fillColorArray);
            drawingTexture.Apply();
        }

        void ApplyTextureToImage()
        {
            Sprite sprite = Sprite.Create(drawingTexture, new Rect(0, 0, drawingTexture.width, drawingTexture.height),
                new Vector2(0.5f, 0.5f));
            imageCanvas.sprite = sprite;
        }

        void Draw(Vector2 position)
        {
            Vector2 localPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRect, position, null, out localPos))
            {
                localPos += new Vector2(imageRect.rect.width / 2f, imageRect.rect.height / 2f);

                if (previousPosition == null) // Если предыдущая позиция не установлена
                {
                    previousPosition = localPos;
                }

                int startX = Mathf.FloorToInt(localPos.x - brushSize / 2f);
                int startY = Mathf.FloorToInt(localPos.y - brushSize / 2f);

                Vector2 startPos = previousPosition.Value;

                for (int i = 0; i < smoothness; i++)
                {
                    float lerpAmount = (float)i / (float)smoothness;
                    Vector2 lerpedPos = Vector2.Lerp(startPos, localPos, lerpAmount);

                    int x = Mathf.FloorToInt(lerpedPos.x - brushSize / 2f);
                    int y = Mathf.FloorToInt(lerpedPos.y - brushSize / 2f);

                    for (int offsetX = 0; offsetX < brushSize; offsetX++)
                    {
                        for (int offsetY = 0; offsetY < brushSize; offsetY++)
                        {
                            int currentX = x + offsetX;
                            int currentY = y + offsetY;

                            if (currentX >= 0 && currentX < drawingTexture.width && currentY >= 0 &&
                                currentY < drawingTexture.height)
                            {
                                drawingTexture.SetPixel(currentX, currentY, brushColor);
                            }
                        }
                    }
                }

                drawingTexture.Apply();
                previousPosition = localPos;
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = null;
            }

            if (Input.GetMouseButton(0))
            {
                Draw(Input.mousePosition);
            }
        }
    }
}