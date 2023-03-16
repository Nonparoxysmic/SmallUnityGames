using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPanelController : MonoBehaviour
{
    public Transform ContentTransform;
    public CreatureTeamPanel Template;

    List<CreatureTeamPanel> _panels;

    void OnEnable()
    {
        if (ContentTransform == null)
        {
            this.Error($"{nameof(ContentTransform)} reference not set in the Inspector.");
            return;
        }
        if (Template == null)
        {
            this.Error($"{nameof(Template)} reference not set in the Inspector.");
            return;
        }
        Template.gameObject.SetActive(false);
        StartCoroutine(Initialize());
    }

    void OnDisable()
    {
        foreach (CreatureTeamPanel panel in _panels)
        {
            Destroy(panel.gameObject);
        }
        _panels.Clear();
    }

    IEnumerator Initialize()
    {
        yield return null;
        _panels = new List<CreatureTeamPanel>();
        foreach (CreatureStats creature in Menagerie.AllCreatures)
        {
            CreatureTeamPanel panel = Instantiate(Template);
            panel.gameObject.SetActive(true);
            panel.SetCreature(creature);
            panel.transform.SetParent(ContentTransform, false);
            _panels.Add(panel);
        }
    }
}
