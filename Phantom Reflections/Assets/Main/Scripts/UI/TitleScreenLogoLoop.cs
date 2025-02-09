using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Main
{
    public class TitleScreenLogoLoop : MonoBehaviour
    {
        private string folderPath = "Logo"; // ���wPNG�ǦC�Ҧb����Ƨ����| (�۹��Resources)
        private float frameRate = 0.05f;

        private Sprite[] frames;
        private Image imageComponent;

        private void Start()
        {
            LoadSprites();
            imageComponent = GetComponent<Image>();
            StartCoroutine(PlayAnimation());
        }
        private void LoadSprites()
        {
            frames = Resources.LoadAll<Sprite>(folderPath)
                              .OrderBy(sprite => sprite.name) // �T�O���W�ٶ��ǱƦC
                              .ToArray();

            if (frames.Length == 0)
            {
                Debug.LogError("��������Sprite�A���ˬd���|�Τ��W�I");
            }
        }

        private IEnumerator PlayAnimation()
        {
            int index = 0;
            while (true)
            {
                imageComponent.sprite = frames[index];
                index++;

                if (index >= frames.Length)
                {
                    yield return new WaitForSeconds(10f);
                    index = 0;
                }
                else
                {
                    yield return new WaitForSeconds(frameRate);
                }
            }
        }
    }
}
