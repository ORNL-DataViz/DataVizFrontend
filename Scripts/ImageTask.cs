using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MyComponent : MonoBehaviour
{
    [System.Serializable]
    public class ImageTask
    {
        public int photoID;
        public Texture2D taskImage;
        public Vector2 taskDimensions;
        public byte[] serializedPhoto;
        public bool taskCorrect;
        public bool userCorrect;
        public bool userUndo = false;
        public Dictionary<String, String> userDecisionPoints = new Dictionary<string, string>();

        public ImageTask(byte[] sp, bool tc)
        {
            serializedPhoto = sp;
            taskCorrect = tc;
        }

        public void unloadTexture()
        {
            taskImage = null;
        }

        public void forceLoadTexture()
        {
            taskImage = new Texture2D(2, 2);
            taskImage.LoadImage(serializedPhoto);
            taskImage.Apply();
        }

        public IEnumerator loadTexture()
        {
            Debug.Log("You got into LoadTexture()");
            // Load the texture
            taskImage = new Texture2D(2, 2);
            taskImage.LoadImage(serializedPhoto);
            taskImage.Apply();

            // Generate it's proper display dimensions
            GenerateResizedRawImageDimensions();

            yield return null;
        }

        public void GenerateResizedRawImageDimensions()
        {
            /// <summary>
            /// Finds the larger edge of the image, calculates the necessary scaling val
            /// </summary>
            /// <param name="nativeDimensions">Original image dimensions</param>
            /// <return>
            /// Vector2 containing downscaled image dimensions
            /// </return>

            Vector2 nativeDimensions = new Vector2(taskImage.width, taskImage.height);
            float targetedArea = 200 * 200;
            if (nativeDimensions.x > nativeDimensions.y)
            {
                float scale = nativeDimensions.y / nativeDimensions.x;
                double newY = Math.Sqrt(targetedArea * scale);
                double newX = targetedArea / newY;
                taskDimensions = new Vector2((float)newX, (float)newY);
            }
            else
            {
                float scale = nativeDimensions.x / nativeDimensions.y;
                double newX = Math.Sqrt(targetedArea * scale);
                double newY = targetedArea / newX;
                taskDimensions = new Vector2((float)newX, (float)newY);
            }
        }

    }

    [System.Serializable]
    public class LinkedList
    {
        internal Node head = null;
        internal Node tail = null;
        internal Node CDP; // Currently Displayed Photo
        internal Node LDP = null; // Last Displayed photo
        internal Node REN; // Rendered Edge Node = Farthest out rendered node
        internal Node PEN; // Planned Edge Node = Furthest queued node to render
        internal float len = 0;
        internal float currentPosition = 0;

        public void Add(MyComponent.Node newImageNode)
        {
            if ((head == null) && (tail == null))
            {
                head = newImageNode;
            }
            else
            {
                if ((head != null) && (tail == null))
                {
                    tail = newImageNode;
                    head.setNext(tail);
                }
                else
                {
                    Node tempNode = newImageNode;
                    tail.setNext(tempNode);
                    tail = tempNode;
                }
            }


        }

        public void stepForward()
        {
            if( PEN != null)
            {
               //StartCoroutine(PEN.loadTexture());
            }

            if ( LDP != null)
            {
                Node tempNode = LDP;
                LDP = CDP;
                CDP = CDP.next;
                tempNode.unloadTexture();
                REN = REN.next;
                PEN = PEN.next;
            }
            else
            {

            }

        }

        public Node getHead()
        {
            return head;
        }

        public float getLen()
        {
            return len;
        }


        // Node.ImageTask Manipulation functions
        private void LoadTexture(Node nodeToLoad)
        {
            nodeToLoad.loadTexture();
        }

        private void UnloadTexture(Node nodeToUnload)
        {
            nodeToUnload.unloadTexture();
        }

        public void StoreUserInput(bool userInput, String decisionPoint)
        {
            LDP.userCorrect = (LDP.taskCorrect == userInput);
            LDP.userDecisionPoints.Add(decisionPoint, userInput.ToString());
        }

        public void StoreUserUndo(String decisionPoint)
        {
            LDP.userUndo = true;
            LDP.userDecisionPoints.Add(decisionPoint, "Undo");
        }

        public float scoreLinkedList()
        {
            Node tempNode = head;
            int score = 0;
            while( tempNode != null)
            {
                if (tempNode.userCorrect)
                {
                    score++;
                }
                tempNode = tempNode.getNext();
            }

            return score / len;

        }

    }

    [System.Serializable]
    public class Node : ImageTask
    {
        internal Node next;

        public Node(Node nextNode, byte[] sp, bool tc) : base(sp, tc)
        {
            next = nextNode;
        }

        public void setNext(Node newNext)
        {
            next = newNext;
        }

        public Node getNext()
        {
            return next;
        }

    }

}
