using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    bool updating;
    Image img;
    private Animator animator;

    public void Initialize(int v, Point p, Sprite piece, AnimatorOverrideController overrideController)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
            animator.runtimeAnimatorController = overrideController;
        }
        value = v;
        SetIndex(p);
        img.sprite = piece;
    }

    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }

    public void ResetPosition()
    {
        pos = new Vector2((rect.rect.width / 2) + (rect.rect.width * index.x), -(rect.rect.width / 2) - (rect.rect.width * index.y));
    }

    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }

    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    public bool UpdatePiece()
    {
        if(Vector3.Distance(rect.anchoredPosition, pos) > 1)
        {
            MovePositionTo(pos);
            updating = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
    }

    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (updating) return;
        MovePieces.instance.MovePiece(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MovePieces.instance.DropPiece();
    }

    public void Animate()
    {
        if (animator != null)
        {
            StartCoroutine(Animating());
        }
    }

    IEnumerator Animating()
    {
        animator.enabled = true;
        animator.SetBool("Animating", true);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("Animating", false);
        animator.enabled = false;
    }
}