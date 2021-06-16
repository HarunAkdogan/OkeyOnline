using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Mirror;
using Model;
using UnityEngine;
using View.Renderer;
using Random = UnityEngine.Random;

namespace Core.Manager
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance;
        [Header("All Tiles")]
        public List<Tile> allTiles = new List<Tile>(); //Butun Taslar

        
        [Header("Table")]
        /*
         * Butun Taslari Tutan Nese.
         * Stack veri yapisinda.
         */
        public Table table;
        public TableController tableController;
        public DragController dragController;

        [Header("Tile Renderer")] public TileRenderer tileRenderer;
        
        [Header("Players")]
        public List<Model.Player> players = new List<Model.Player>();


        private void Awake()
        {
            instance = this;
        }

        public void PlayersAdd(Model.Player playersPlay)
        {
            players.Add(playersPlay);
        }
        
        
        private void Start()
        {
            StartCoroutine(InitGame());
        }

        
        /// <summary>
        /// Oyunun basladiginda neler olacagi.
        /// </summary>
        
        IEnumerator InitGame()
        {
            while (players.Count <= 0)
            {
                yield return null;
            }
            
            CreateAllTiles(); //Butun Taslari Olustur
            SetFalseJokers();
            Shuffler(allTiles);
            CreateTableTiles(); //Taslari Stacke ekle.
            StartGame();
        }

        private void StartGame()
        {
            players[0].GiveTiles(tableController.GetNTile(15));
            
            (players[0] as Model.Player).GetDeals();
        }

        /// <summary>
        /// All Stones Created
        /// </summary>
        private void CreateAllTiles()
        {
            for (int loops = 0; loops < 2; loops++)
            {
                for (int colors = 0; colors < 4; colors++)
                {
                    for (int number = 1; number < 14; number++)
                    {
                        allTiles.Add(new Tile(number,number, Tile.IntToTileColor(colors), false,false));
                        
                    }
                }
            }
        }
        
        /// <summary>
        /// Gostergenin, Okeylerin ve Sahte Okeylerin belirlenmes
        /// </summary>
        void SetFalseJokers(){
            
            //Todo Bagimlilik kontrolu!
            Tile jokerTile = SetJoker();
             for (int i = 0; i < 2; i++)
             {
                 Tile falseTile = new Tile(jokerTile.number, jokerTile.number, jokerTile.color,false,true);
                 allTiles.Add(falseTile);
                 Debug.LogWarning("FalseJoker Tile -> " + falseTile.number + ", " + falseTile.color);
             }
        }


        /// <summary>
        /// Jokerin Belirlenmesi
        /// </summary>
        Tile SetJoker()
        {
            Tile ind = allTiles[Random.Range(0, 104)];
            tileRenderer.tile = ind;
            tileRenderer.Render();
            Tile JokerTile = null;
            Debug.LogWarning("Indicator Tile -> " + ind.number + ", " + ind.color);
            
            foreach (Tile tile in allTiles)
            {
                if (((ind.number == 13 && tile.number == 1) || (ind.number == tile.number - 1)) && ind.color == tile.color)
                {
                    tile.isJoker = true;
                    JokerTile = tile;
                }
            }

            allTiles.Remove(ind);
            
            Debug.LogWarning("Joker Tile -> " + JokerTile.number + ", " + JokerTile.color);
            return JokerTile;
        }
        
        
        /// <summary>
        /// Butun Taslarin Table Stackine eklenmesi
        /// </summary>
        private void CreateTableTiles()
        {
            tableController.GiveTile(allTiles);
        }

        /// <summary>
        /// Taslari Karistirir
        /// </summary>
        public void Shuffler(List<Tile> list)
        {
            int n = list.Count - 1;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n);
                Tile value = list[k];
                list[k] = list[n];
                list[n] = value;
                n--;
            }
        }
        
    }
}