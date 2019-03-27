using UnityEngine;
using UnityEngine.UI;
using FixMath;
using FixMath.UniRx;
using UniRx;
using System;

public class DemoUniRxFixMath : MonoBehaviour
{
	public F32ReactiveProperty rx;

	public Text text_rx;
	public Button button_random;

	void Awake()
	{
		Debug.Assert(text_rx != null);
		Debug.Assert(button_random != null);
	}

	void Start()
	{
		rx = new F32ReactiveProperty(F32.One);

		rx.Subscribe(v =>
		{
			text_rx.text = $"rx: {v.Float}";
		}).AddTo(gameObject);

		button_random.OnClickAsObservable()
		.Subscribe(_ => SetAsRandom())
		.AddTo(gameObject);
	}

	void SetAsRandom()
	{
		rx.Value = new F32(UnityEngine.Random.Range(0.0f, 100.0f));
	}
}
