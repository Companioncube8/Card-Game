using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour {

    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    private memoryCard _firstRevealed;
    private memoryCard _secondRevealed;

    private int _score = 0;

    [SerializeField] private memoryCard originalCard;
    [SerializeField] private Sprite[] image;
    [SerializeField] private TextMesh scoreLabel;

    public void Restart()
    {
        SceneManager.LoadScene("1");
    }

    void Start () {
        Vector3 startPos = originalCard.transform.position;

        scoreLabel.text = "Score: " + _score;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3};

        numbers = ShuffleArray();


        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                memoryCard card;
                if (i == 0 && j == 0)
                    card = originalCard;
                else
                    card = Instantiate(originalCard) as memoryCard;

                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, image[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetX * j) + startPos.y;

                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
	}

    private int[] ShuffleArray()
    {
        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3};
        for (int i = 0; i < numbers.Length; i++)
        {
            int r = Random.Range(i, numbers.Length);
            int tmp = numbers[i];
            numbers[i] = numbers[r];
            numbers[r] = tmp;
        }
        return numbers;
    }

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    public void CardRevealed(memoryCard card)
    {
        if (_firstRevealed == null)
            _firstRevealed = card;
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = null;
        _secondRevealed = null;
    }

}
