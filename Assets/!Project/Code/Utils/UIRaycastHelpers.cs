using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public static class UIRaycastHelpers
{
    public static bool IsPointerOverClickableUI()
	{
		return IsPointerOverClickableUI(string.Empty);
	}
    
    public static bool IsPointerOverUI()
    {
        return IsPointerOverUI(string.Empty);
	}
    
    public static bool IsPointerOverClickableUI(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        results = results.Where(r => r.module is GraphicRaycaster) as List<RaycastResult>;

        if (!string.IsNullOrEmpty(tag))
			results = results.Where(r => r.gameObject.CompareTag(tag)).ToList();

		return results.Any(r => r.gameObject.GetComponent<IPointerClickHandler>() != null);
    }
    
    public static bool IsPointerOverUI(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

		if (!string.IsNullOrEmpty(tag))
			results = results.Where(r => r.gameObject.CompareTag(tag)).ToList();

		return results.Any(r => r.module is GraphicRaycaster);
    }
}