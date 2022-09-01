using UnityEngine;
using UnityEngine.UI;

public class TikTacGameStarter : MonoBehaviour
{
    [SerializeField] GameObject _optionMenu;
    [SerializeField] Dropdown _SizeField;
    [SerializeField] ToggleGroup _playMode;
    [SerializeField] Toggle _aiGetFirst;
    [SerializeField] TikTacGameManager _gameManager;

    public void StartPlay()
    {
        int SizeField = _SizeField.value + 3;

        PlayMode playMode = PlayMode.PVP;

        if (_playMode.GetFirstActiveToggle().name == "PVP") playMode = PlayMode.PVP;
        if (_playMode.GetFirstActiveToggle().name == "PVE") playMode = PlayMode.PVE;
        if (_playMode.GetFirstActiveToggle().name == "EVE") playMode = PlayMode.EVE;

        bool playerGetFirst = !_aiGetFirst.isOn;
        _optionMenu.SetActive(false);

        _gameManager.StartGame(playMode, SizeField, playerGetFirst);
    }
}
