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

	public int resources = 0;

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
		} else {
			Debug.Log("Cannot aford. need: " + card.cost + " have: " + resources);
		}
	}

	public void playCard(Card card) {
		if (resources >= card.cost && board.spawnWalls(card.walls, card.towers)) {
			resources -= card.cost;
			cards.Remove(card);
			Destroy(card.gameObject);
			ReposistionCards();
		}
	}

	// Update is called once per frame
	void Update () {
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
