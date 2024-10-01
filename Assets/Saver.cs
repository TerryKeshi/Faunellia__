using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public void Save()
    {
        GameObject go = GameObject.FindGameObjectWithTag("AllFather");
        AllFather allFather = go.GetComponent<AllFather>();
        allFather.SaveTheSave();

        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas"); // �������� Canvas
        Transform canvasTransform = canvas.transform; // �������� Transform Canvas
        // ����� ������ "Game saved" � �������� �������� Canvas
        Transform goTransform = canvasTransform.Find("Game saved");
        GameObject go2 = goTransform.gameObject;
        go2.SetActive(true);

        StartCoroutine(HideAfterDelay(go2, 1f)); // ��������� �������� ��� ������� ������� ����� 1 �������
    }

    private IEnumerator HideAfterDelay(GameObject go2, float delay)
    {
        yield return new WaitForSeconds(delay);

        go2.SetActive(false);
    }
}
