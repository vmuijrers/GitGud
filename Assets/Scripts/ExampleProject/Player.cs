﻿namespace ExampleProject
{
    public class Player : CustomMonoBehaviour
    {
        public event System.Action JumpEvent;

        void Start ()
        {
            
        }

        public void Jump()
        {
            //Do your jump here
            //rigidBody.AddForce(...);
            //Raise Jump Event for reactions
            JumpEvent?.Invoke();
        }
    }

}
