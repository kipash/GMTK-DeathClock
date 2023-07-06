using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarSettings : ScriptableObject
{
    [System.Serializable]
    public class LinkData
    {
        public string Name;
        public string URL;
    }

    public LinkData[] CustomLinks = new LinkData[]
    {
        new LinkData() { Name = "Give cat!", URL = "https://cataas.com/cat" },
        new LinkData() { Name = "Give dog!", URL = "https://random.dog/" },
        new LinkData() { Name = "GMTK Itch",  URL ="https://itch.io/jam/gmtk-2023" },
        new LinkData() { Name = "My Itch",  URL ="https://itch.io/" },
    };

    public bool ListScenes = true;

    public string DeadlineDateTime = "2023/07/09_17:00:00";
    public string Theme = "unannounced";
}