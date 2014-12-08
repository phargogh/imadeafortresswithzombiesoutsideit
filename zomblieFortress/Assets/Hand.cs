using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

	public GameObject cardFab;
	public Board board;


	public List<Card> cards = new List<Card>();
	public int handSize = 5;

	public float leftCardPosition = -12f;
	public float cardSpacing = 6f;

	private int resources = 300;

	public Card selected;

	// Use this for initialization
	void Start () {
	
	}

	public void SelectCard (Card card) {
		if (resources >= card.cost) {
			selected = card;
			card.select();
			foreach (Card c in cards) {
				if (c.isSelected && c != selected) c.unselect();
			}
            audio.Play();
		} else {
			Debug.Log("Cannot aford. need: " + card.cost + " have: " + resources);
		}
	}

    public int get_resources() {
        return this.resources;
    }

    public void add_resources(int new_resources) {
        this.resources += new_resources;
    }

	public void Unselect() {
		selected.unselect ();
		selected = null;
	}

	public void playCard(Card card, Point pos) {
		if (resources >= card.cost && board.spawnWalls(card.walls, card.towers, pos)) {
			resources -= card.cost;
			cards.Remove(card);
			Destroy(card.gameObject);
			ReposistionCards();
            audio.Play();
		}
	}

	// Update is called once per frame
	void Update () {
		if (selected && Input.GetMouseButtonUp(0)) {
			Point gridPos =  board.worldPosToGridPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			playCard(selected, gridPos);
		}
		if (cards.Count < handSize) {
			Debug.Log("Create a card");
			Vector3 pos = GetCardPosition(cards.Count + 1);
			GameObject cardOjb = (GameObject) Instantiate(cardFab, pos, Quaternion.identity);
			Card card = cardOjb.GetComponent<Card>();
			card.init(board, this);
			cards.Add(card);
			ReposistionCards();
		}
	}

	void ReposistionCards() {
		int count = 0;
		foreach (Card c in cards) {
			c.gameObject.transform.position = GetCardPosition(count);
			count++;
		}
	}

	Vector3 GetCardPosition(int count) {
		return new Vector3 (leftCardPosition + cardSpacing * count, gameObject.transform.position.y, -3);
	}
}
