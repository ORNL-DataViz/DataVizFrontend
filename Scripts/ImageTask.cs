using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
        public Dictionary<String, DateTime> userDecisionPoints;

        public void UnloadTexture()
        {
            taskImage = null;
        }

        public void LoadTexture()
        {
            taskImage = new Texture2D(2, 2);
            taskImage.LoadImage(serializedPhoto);
            taskImage.Apply();
        }

     
    }

    [System.Serializable]
    public class LinkedList
    {
        internal Node head = null;
        internal Node tail = null;
        internal Node CDP; // Currently Displayed Photo
        internal Node LDP; // Last Displayed photo
        internal Node REN; // Rendered Edge Node = Farthest out rendered node
        internal Node PEN; // Planned Edge Node = Furthest queued node to render


        public void Add(MyComponent.ImageTask newImageNode)
        {
            if ((head == null) && (tail == null))
            {
                head = new Node(newImageNode, null);
            }
            else
            {
                if ((head != null) && (tail == null))
                {
                    tail = new Node(newImageNode, null);
                    head.addNext(tail);
                }
                else
                {
                    Node tempNode = new Node(newImageNode, null);
                    tail.addNext(tempNode);
                    tail = tempNode;
                }
            }


        }

        public Node getHead()
        {
            return head;
        }


        // Node.ImageTask Manipulation functions
        private void LoadTexture(Node nodeToLoad)
        {
            nodeToLoad.nodeImage.LoadTexture();
        }

        private void UnloadTexture(Node nodeToUnload)
        {
            nodeToUnload.nodeImage.UnloadTexture();
        }

        private void StoreUserInput(Node nodeToStoreTo, bool userInput, DateTime decisionPoint)
        {
            nodeToStoreTo.nodeImage.userCorrect = (nodeToStoreTo.nodeImage.taskCorrect == userInput);
            nodeToStoreTo.nodeImage.userDecisionPoints.Add(userInput.ToString(), decisionPoint);
        }

        private void StoreUserUndo(Node nodeToStoreTo, DateTime decisionPoint)
        {
            nodeToStoreTo.nodeImage.userUndo = true;
            nodeToStoreTo.nodeImage.userDecisionPoints.Add("undo", decisionPoint);
        }
    }

    [System.Serializable]
    public class Node
    {
        internal ImageTask nodeImage;
        internal Node next;

        public Node(MyComponent.ImageTask newNodeImage, Node nextNode)
        {
            nodeImage = newNodeImage;
            next = null;
        }

        public void addNext(Node newNext)
        {
            next = newNext;
        }

        public Node getNext()
        {
            return next;
        }
    }

}
