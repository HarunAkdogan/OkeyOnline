using System;
using Mirror;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Helpers;

namespace View.Renderer
{
    [Serializable]
    public class TileRenderer : NetworkBehaviour
    {
        
        [SyncVar]
        public Tile tile;
        
        public TextMeshProUGUI id;
        public GameObject knob;
        public Image knobColor;
        

        public void Render()
        {
            if (tile.number == 0)
            {
                id.text = "";
                knob.SetActive(false);
            }
            else
            { 
            
                id.text = tile.number.ToString();
                id.color = TileColorTypeToColor.TileColorToColor(tile.color);
                knob.SetActive(true);
                knobColor.color = TileColorTypeToColor.TileColorToColor(tile.color);
            }
            
        }
        
       
        
    }
}