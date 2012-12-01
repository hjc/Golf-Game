using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using BEPUphysics.Entities;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Collidables;
using BEPUphysics.MathExtensions;

namespace GettingStartedDemo
{
    //will rotate our level model for us, giving us the desired level each time
    public class LevelManager
    {

        public Vector3 startingPos = new Vector3(0,0,0);

        //each hole has a start box that identifies where it is in the master
        // hole model. The ball starts from here, store them all.
        public Vector3[] startBoxes = new Vector3[8];

        public LevelManager()
        {
            //populate the various positions of our holes' starting boxes
            startBoxes[0] = new Vector3(14, 15, 16);
        }

        //beat a level; get ready for the next by incrementing current level
        // and then loading the next
        //private void nextLevel() {
        //    CurrentLevel += 1;

        //    if (CurrentLevel > Levels.Count)
        //        //win
        //        return;
        //    else
        //    {
        //        //first remove current level
        //        this.theGame.removeModel(CurrentLevelEntity);
        //        //load next level

        //        //set level to current level
        //        return;
        //    }
        //}

        
        //public void setupCurrentLevel()
        //{
        //    AddModelLevel(this.Levels[0]);
        //}
    }
}
