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
            taskImage = new Texture2D(2, 2);
            taskImage.LoadImage(serializedPhoto);
            taskImage.Apply();
            yield return null;
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
        internal float len = 0;
        internal float currentPosition = 0;

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

        public void stepForward()
        {
            Node tempNode = LDP;
            LDP = CDP;
            CDP = CDP.next;
            tempNode.nodeImage.unloadTexture();
            REN = REN.next;
            PEN = PEN.next;
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
            nodeToLoad.nodeImage.loadTexture();
        }

        private void UnloadTexture(Node nodeToUnload)
        {
            nodeToUnload.nodeImage.unloadTexture();
        }

        public void StoreUserInput(bool userInput, String decisionPoint)
        {
            LDP.nodeImage.userCorrect = (LDP.nodeImage.taskCorrect == userInput);
            LDP.nodeImage.userDecisionPoints.Add(decisionPoint, userInput.ToString());
        }

        public void StoreUserUndo(String decisionPoint)
        {
            LDP.nodeImage.userUndo = true;
            LDP.nodeImage.userDecisionPoints.Add(decisionPoint, "Undo");
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

        public Texture2D getNodeText()
        {
            return nodeImage.taskImage;
        }
    }

}
