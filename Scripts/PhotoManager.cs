using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Basic Doubly Linked list structure pulled from:
    // https://dzone.com/articles/linked-list-implementation-in-c
    // mostly because I was too lazy to write my own and vet it - SA :P

    internal class Node
    {

        internal MyComponent.ImageTask nodeImage;
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

    }

    internal class DoubleLinkedList
    {
        internal Node head;
        internal Node tail;
        internal Node CDP; // Currently Displayed Photo
        internal Node LDP; // Last Displayed photo
        internal Node REN; // Rendered Edge Node = Farthest out rendered node
        internal Node PEN; // Planned Edge Node = Furthest queued node to render


        private void Add (MyComponent.ImageTask newImageNode)
        {
            if( ( head == null ) && ( tail == null))
            {
                head = new Node(newImageNode, null);
            }
            else
            {
                if( ( head != null ) && ( tail == null))
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
}
