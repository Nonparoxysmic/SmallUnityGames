using UnityEngine;
using UnityEngine.UI;

public class CreatureTeamPanel : MonoBehaviour
{
    CreatureStats _creature;
    bool _onTeamC;
    bool _onTeamH;

    Image _creatureImage;
    Text _creatureName;
    Text _speciesName;
    Text _teamLabel;
    Button _buttonC;
    Button _buttonH;

    Image _buttonImageC;
    Image _buttonImageH;
    Text _buttonTextC;
    Text _buttonTextH;

    void OnEnable()
    {
        _creatureImage = this.FindComponentInChildren<Image>("Creature Sprite");
        if (_creatureImage == null)
        {
            this.Error($"Missing or unavailable \"Creature Sprite\" Image component.");
            return;
        }
        _creatureName = this.FindComponentInChildren<Text>("Creature Name");
        if (_creatureName == null)
        {
            this.Error($"Missing or unavailable \"Creature Name\" Text component.");
            return;
        }
        _speciesName = this.FindComponentInChildren<Text>("Species Name");
        if (_speciesName == null)
        {
            this.Error($"Missing or unavailable \"Species Name\" Text component.");
            return;
        }
        _teamLabel = this.FindComponentInChildren<Text>("Team Label");
        if (_teamLabel == null)
        {
            this.Error($"Missing or unavailable \"Team Label\" Text component.");
            return;
        }
        _buttonC = this.FindComponentInChildren<Button>("Team C Button");
        if (_buttonC == null)
        {
            this.Error($"Missing or unavailable \"Team C Button\" Button component.");
            return;
        }
        _buttonH = this.FindComponentInChildren<Button>("Team H Button");
        if (_buttonH == null)
        {
            this.Error($"Missing or unavailable \"Team H Button\" Button component.");
            return;
        }
        _buttonImageC = _buttonC.GetComponent<Image>();
        _buttonImageH = _buttonH.GetComponent<Image>();
        _buttonTextC = _buttonC.transform.GetComponentInChildren<Text>();
        _buttonTextH = _buttonH.transform.GetComponentInChildren<Text>();
    }

    void OnDisable()
    {
        _buttonC.onClick.RemoveAllListeners();
        _buttonH.onClick.RemoveAllListeners();
    }

    public void SetCreature(CreatureStats creature)
    {
        _creature = creature;
        _creatureImage.sprite = creature.Species.BaseSprite;
        _creatureImage.color = creature.Species.BaseColor;
        if (creature.Species.Size == Size.Medium)
        {
            _creatureImage.transform.localScale = new Vector3(0.5f, 0.5f);
        }
        if (creature.Species.Size == Size.Small)
        {
            _creatureImage.transform.localScale = new Vector3(0.25f, 0.25f);
        }
        _creatureName.text = creature.IndividualName;
        _speciesName.text = "Species: " + creature.Species.DisplayName;
        _onTeamC = Menagerie.ComputerTeam.Contains(creature);
        if (_onTeamC)
        {
            _buttonImageC.color = Color.green;
            _buttonTextC.text = "-";
        }
        _onTeamH = Menagerie.HumanTeam.Contains(creature);
        if (_onTeamH)
        {
            _buttonImageH.color = Color.green;
            _buttonTextH.text = "-";
        }
        UpdateTeamLabel();
        _buttonC.onClick.AddListener(OnClickC);
        _buttonH.onClick.AddListener(OnClickH);
    }

    private void OnClickC()
    {
        if (Menagerie.ComputerTeam.Contains(_creature))
        {
            if (Menagerie.ComputerTeam.Count > 1)
            {
                Menagerie.ComputerTeam.Remove(_creature);
                _buttonImageC.color = Color.white;
                _buttonTextC.text = "+";
                _onTeamC = false;
            }
        }
        else
        {
            Menagerie.ComputerTeam.Add(_creature);
            _buttonImageC.color = Color.green;
            _buttonTextC.text = "-";
            _onTeamC = true;
        }
        UpdateTeamLabel();
    }

    private void OnClickH()
    {
        if (Menagerie.HumanTeam.Contains(_creature))
        {
            if (Menagerie.HumanTeam.Count > 1)
            {
                Menagerie.HumanTeam.Remove(_creature);
                _buttonImageH.color = Color.white;
                _buttonTextH.text = "+";
                _onTeamH = false;
            }
        }
        else
        {
            Menagerie.HumanTeam.Add(_creature);
            _buttonImageH.color = Color.green;
            _buttonTextH.text = "-";
            _onTeamH = true;
        }
        UpdateTeamLabel();
    }

    void UpdateTeamLabel()
    {
        if (_onTeamC && _onTeamH)
        {
            _teamLabel.text = "Team: Both";
        }
        else if (_onTeamC)
        {
            _teamLabel.text = "Team: C";
        }
        else if (_onTeamH)
        {
            _teamLabel.text = "Team: H";
        }
        else
        {
            _teamLabel.text = "Team: None";
        }
    }
}
