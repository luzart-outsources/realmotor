using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static AirFishLab.ScrollingList.ListBank;

public class LeaderBoardUIInGame : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform parentSpawn;
    public ItemLeaderBoardUI itemLeaderBoardUIPf;
    public List<ItemLeaderBoardUI> listItemLeaderBoardUI = new List<ItemLeaderBoardUI>();
    private List<DB_LeaderBoardInGame> _listDBInGame = new List<DB_LeaderBoardInGame>();
    private ItemLeaderBoardUI itemMe;
    [ShowInInspector]
    public float GetVerticle;
    public bool IsSetVerticle = false;
    [ShowInInspector]
    public float SetVerticle;
    private void Update()
    {
        GetVerticle = scrollRect.verticalNormalizedPosition;
        if (IsSetVerticle)
        {
            scrollRect.verticalNormalizedPosition = SetVerticle ;
        }
    }
    public void InitList(List<DB_LeaderBoardInGame> list)
    {
        this._listDBInGame = list;
        int length = list.Count;
        MasterHelper.InitListObj<ItemLeaderBoardUI>(length, itemLeaderBoardUIPf, listItemLeaderBoardUI, parentSpawn, (item, index) =>
        {
            item.gameObject.SetActive(true);
            var db = _listDBInGame[index];
            item.InitItem(db);
            if(db.eTeam == ETeam.Player)
            {
                itemMe = item;
            }
        });

    }

    public void UpdateList(List<DB_LeaderBoardInGame> list)
    {
        _listDBInGame = list;
        int length = _listDBInGame.Count;
        for (int i = 0; i < length; i++)
        {
            var item = listItemLeaderBoardUI[i];
            var db = _listDBInGame[i];
            item.InitItem(db);
            if(db.eTeam == ETeam.Player)
            {
                itemMe = item;
            }
        }
        if (itemMe != null)
        {
            scrollRect.FocusOnRectTransform(itemMe.rectTransform);
        }

    }
    public void ScrollTo(RectTransform target)
    {
        // Tính toán vị trí cần cuộn đến
        Canvas.ForceUpdateCanvases();
        Vector2 contentPosition = (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

        // Giới hạn vị trí cuộn trong phạm vi cho phép của content
        float minY = scrollRect.transform.InverseTransformPoint(scrollRect.content.position).y - (scrollRect.content.rect.height - scrollRect.viewport.rect.height);
        float maxY = scrollRect.transform.InverseTransformPoint(scrollRect.content.position).y;
        contentPosition.y = Mathf.Clamp(contentPosition.y, minY, maxY);

        scrollRect.content.position = contentPosition;
        // Bắt đầu cuộn đến vị trí tính toán
        //StartCoroutine(ScrollToPosition(contentPosition, duration));
    }
    public void FocusOnItem(RectTransform targetItem)
    {
        // Bắt buộc cập nhật bố cục để có kích thước chính xác
        Canvas.ForceUpdateCanvases();

        // Tính toán vị trí của mục tiêu so với nội dung
        RectTransform contentRect = scrollRect.content;
        Vector2 targetPosition = (Vector2)contentRect.InverseTransformPoint(targetItem.position);
        Vector2 contentPosition = contentRect.anchoredPosition;

        // Tính toán vị trí cần cuộn tới
        float newY = contentPosition.y + targetPosition.y - (scrollRect.viewport.rect.height / 2) + (targetItem.rect.height / 2);

        // Đảm bảo vị trí cuộn nằm trong giới hạn cho phép
        newY = Mathf.Clamp(newY, 0, contentRect.rect.height - scrollRect.viewport.rect.height);

        // Cập nhật vị trí của nội dung
        contentRect.anchoredPosition = new Vector2(contentPosition.x, newY);
    }
    //void KeepItemVisible(RectTransform itemToKeepInView)
    //{
    //    Canvas.ForceUpdateCanvases();
    //    Vector3[] itemCorners = new Vector3[4];
    //    itemToKeepInView.GetWorldCorners(itemCorners);
    //    Vector3[] viewCorners = new Vector3[4];
    //    scrollRect.viewport.GetComponent<RectTransform>().GetWorldCorners(viewCorners);
    //    float difference = 0;
    //    if (itemCorners[1].y > viewCorners[1].y)
    //    {
    //        difference = itemCorners[1].y - viewCorners[1].y;
    //    }
    //    else if(itemCorners[0].y < viewCorners[0].y)
    //    {
    //        difference = itemCorners[0].y - viewCorners[0].y;
    //    }
    //    float height = viewCorners[1].y - viewCorners[0].y;
    //    float normalizedDifference = difference / height;
    //    Vector2 posCurrent = scrollRect.content.anchoredPosition;
    //    Vector2 size = scrollRect.content.sizeDelta;
    //    scrollRect.content.anchoredPosition = new Vector2(posCurrent.x,posCurrent.y - normalizedDifference* size.y);
    //}

}
