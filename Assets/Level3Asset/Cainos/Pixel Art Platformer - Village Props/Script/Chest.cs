using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;

        // Oyuncunun sandýðýn yanýnda olup olmadýðýný tutan deðiþken
        private bool isPlayerInRange = false;

        private void Update()
        {
            // Eðer oyuncu menzildeyse VE E tuþuna basýldýysa VE sandýk henüz açýk deðilse
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
            {
                Open();
            }
        }

        // Oyuncu sandýðýn alanýna girdiðinde
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) // Çarpan objenin tag'i "Player" ise
            {
                isPlayerInRange = true;
            }
        }

        // Oyuncu sandýðýn alanýndan çýktýðýnda
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }

        [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }
    }
}