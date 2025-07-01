namespace Luzart
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class UIController : BaseController
    {
        public ButtonTopDown btnUp;
        public ButtonTopDown btnDown;
        public ButtonTopDown btnLeft;
        public ButtonTopDown btnRight;
    
        public override void Initialized(BaseMotorbike baseMotorbike)
        {
            base.Initialized(baseMotorbike);
            btnUp.AddListener(MoveUp, UnVerticle);
            btnDown.AddListener(Brake, UnVerticle);
            btnLeft.AddListener(MoveLeft, UnHorizontal);
            btnRight.AddListener(MoveRight, UnHorizontal);
        }
        public override void MoveRight()
        {
            btnLeft.ForcePointUp();
            base.MoveRight();
        }
        public override void MoveLeft()
        {
            btnRight.ForcePointUp();
            base.MoveLeft();
        }
        private void OnEnable()
        {
            btnUp.ForcePointUp();
            btnDown.ForcePointUp();
            btnLeft.ForcePointUp();
            btnRight.ForcePointUp();
        }
    }
}
