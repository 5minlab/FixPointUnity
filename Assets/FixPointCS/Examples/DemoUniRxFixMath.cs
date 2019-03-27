using UnityEngine;
using UnityEngine.UI;
using FixMath;
using FixMath.UniRx;
using UniRx;
using System;

public class DemoUniRxFixMath : MonoBehaviour
{
	[Serializable]
	public class Inner
	{
		public F32ReactiveProperty val;
	}

	[Serializable]
	public class Deep
	{
		public Inner container;
	}

	public F32ReactiveProperty root;
	public Inner inner;
	public Deep deep;

	public Text text_root;
	public Text text_inner;
	public Text text_deep;
	public Button button_random;

	void Awake()
	{
		Debug.Assert(text_root != null);
		Debug.Assert(text_inner != null);
		Debug.Assert(text_deep != null);
		Debug.Assert(button_random != null);
	}

	void Start()
	{
		root = new F32ReactiveProperty(F32.One);
		inner.val = new F32ReactiveProperty(F32.One);
		deep.container.val = new F32ReactiveProperty(F32.One);

		root.Subscribe(v => text_root.text = $"root: {v.Float}").AddTo(gameObject);
		inner.val.Subscribe(v => text_inner.text = $"inner: {v.Float}").AddTo(gameObject);
		deep.container.val.Subscribe(v => text_deep.text = $"deep: {v.Float}").AddTo(gameObject);

		button_random.OnClickAsObservable()
		.Subscribe(_ => SetAsRandom())
		.AddTo(gameObject);
	}

	void SetAsRandom()
	{
		root.Value = new F32(UnityEngine.Random.Range(0.0f, 100.0f));
		inner.val.Value = new F32(UnityEngine.Random.Range(0.0f, 100.0f));
		deep.container.val.Value = new F32(UnityEngine.Random.Range(0.0f, 100.0f));
	}
}
