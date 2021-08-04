using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Mirror;
using Model;
using UnityEditor;
using UnityEngine;
using View.Renderer;
using Random = UnityEngine.Random;

namespace Core.Manager
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance;
        [Header("All Tiles")]
        
        // public List<Tile> allTiles = new List<Tile>(); //Butun Taslar
        readonly public SyncList<Tile> allTiles = new SyncList<Tile>();
        
        [Header("Table")]
        /*
         * Butun Taslari Tutan Nese.
         * Stack veri yapisinda.
         */
        public Table table;
        

        [Header("Tile Renderer")] public TileRenderer tileRenderer;

        [Header("Tile Indicator")]  [SyncVar] public Tile ind;
        
        [Header("Players")]
        public List<Model.Player> players = new List<Model.Player>();


        [Header("EnemyFields")] 
        public PlayerTileField playerTileField;
        public PlayerTileField opponentTileField;
        public PlayerTileField opponentTileField2;
        public PlayerTileField opponentTileField3;
        
        [Header("Game Loop Seconds")]
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;   
        
        private void Awake()
        {
            instance = this;
        }

        
        public void PlayersAdd(Model.Player playersPlay)
        {
            players.Add(playersPlay);
        }
        
        [ServerCallback]
        private void Start()
        {
            m_StartWait = new WaitForSeconds(0.5f);
            m_EndWait = new WaitForSeconds(0.5f);
            StartCoroutine(InitGame());
        }

        
        /// <summary>
        /// Oyunun basladiginda neler olacagi.
        /// </summary>
        
        IEnumerator InitGame()
        {
            while (players.Count < 1)
            {
                yield return null;
            }

            
            yield return StartCoroutine(GetPlayerSettings());
            yield return StartCoroutine(CreateAllTiles());
            yield return StartCoroutine(SetFalseJokers());
            yield return StartCoroutine(Shuffler(allTiles));
            yield return StartCoroutine(CreateTableTiles()); //Taslari Stacke ekle.
            yield return StartCoroutine(StartGame());
        }

        IEnumerator GetPlayerSettings()
        {
            RpcGetPlayerSettings();
            yield return m_StartWait;
        }

        [ClientRpc]
        void RpcGetPlayerSettings()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].playerId = i + 1;

                switch (players[i].playerId)
                {
                    case 1:
                        players[i].playerType = PlayerType.Player1; 
                        break;
                    case 2: 
                        players[i].playerType = PlayerType.Player2;
                        break;
                    case 3: 
                        players[i].playerType = PlayerType.Player3;
                        break;
                        
                    case 4: 
                        players[i].playerType = PlayerType.Player4;
                        break;
                    default: break;
                }

                players[i].AddOpponents(players);
            }
        }
        
        private IEnumerator StartGame()
        {
            // RpcStartGame();

            for (int i = 0; i < players.Count; i++)
            {
                if(i==0)
                    table.GetNTile(15, players[i]);
                else
                    table.GetNTile(14, players[i]);
            }

                yield return m_StartWait;
        }

        [ClientRpc]
        private void RpcStartGame()
        {
            
            // players[0].GiveTiles(table.GetNTile(15));
            
            // players[1].GiveTiles(tableController.GetNTile(14));
            Debug.Log("RpcStartGame");
            
            (players[0] as Model.Player).GetDeals();
            // (players[1] as Model.Player).GetDeals();
        }

        /// <summary>
        /// All Stones Created
        /// </summary>
        private IEnumerator CreateAllTiles()
        {
            if (isServer)
            {
                for (int loops = 0; loops < 2; loops++)
                {
                    for (int colors = 0; colors < 4; colors++)
                    {
                        for (int number = 1; number < 14; number++)
                        {
                            int id = Random.Range(100, 1000);
                            allTiles.Add(new Tile(id,number, Tile.IntToTileColor(colors), false,false));
                        }
                    }
                }
                
            }
            RpcCreateAllTiles();
            yield return m_StartWait;
        }

        [ClientRpc]
        private void RpcCreateAllTiles()
        {
            
            Debug.Log("RpcCreateAllTiles");
        }

        /// <summary>
        /// Gostergenin, Okeylerin ve Sahte Okeylerin belirlenmes
        /// </summary>
        IEnumerator SetFalseJokers()
        {
            
            Tile jokerTile = SetJoker();
            tileRenderer.tile = ind;
            Debug.LogWarning("Ind Tile -> " + ind.number + ", " + ind.color);
            for (int i = 0; i < 2; i++)
            {
                Tile falseTile = new Tile(jokerTile.number, jokerTile.number, jokerTile.color,false,true);
                allTiles.Add(falseTile);
                Debug.LogWarning("FalseJoker Tile -> " + falseTile.number + ", " + falseTile.color);
            }
            yield return m_StartWait;
            
            RpcSetFalseJokers();
            
        }

        [ClientRpc]
        private void RpcSetFalseJokers()
        {
            //Todo Bagimlilik kontrolu!
            tileRenderer.Render();
            Debug.Log("RpcSetFalseJokers => " + ind.number.ToString());
            
        }

        [Command(requiresAuthority = false)]
        public void RemovePlayerTile(Tile tile, Model.Player player)
        {
            if (isServer) RpcRemovePlayerTile(tile, player);
        }

        [ClientRpc]
        private void RpcRemovePlayerTile(Tile tile, Model.Player player)
        {
            player.tiles.Remove(tile);
        }

        /// <summary>
        /// Jokerin Belirlenmesi
        /// </summary>
        Tile SetJoker()
        {
            ind = allTiles[Random.Range(0, 104)];
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
        IEnumerator CreateTableTiles()
        {
            RpcCreateTableTiles();
            table.GiveTile(allTiles);
            yield return m_StartWait;
        }

        [ClientRpc]
        private void RpcCreateTableTiles()
        {
            
            Debug.Log("RpcCreateTableTiles");
        }

        /// <summary>
        /// Taslari Karistirir
        /// </summary>
        public IEnumerator Shuffler(SyncList<Tile> list)
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

            yield return m_StartWait;
        }
        
    }
}