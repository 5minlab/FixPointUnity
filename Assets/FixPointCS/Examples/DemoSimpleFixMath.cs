using UnityEngine;
using UnityEngine.UI;
using FixMath;
using FixMath.Utils;
using UniRx;

public class DemoSimpleFixMath : MonoBehaviour
{
	[F32Raw]
	public F32 raw = new F32(2.5f);
	public F32 val = new F32(1.25f);

	public Text text_raw;
	public Text text_val;
	public Button button_random;

	void Awake()
	{
		Debug.Assert(text_raw != null);
		Debug.Assert(text_val != null);
		Debug.Assert(button_random != null);
	}

	void Start()
	{
		val = F32.One;
		raw = F32.One;

		button_random.OnClickAsObservable()
		.Subscribe(_ => SetAsRandom())
		.AddTo(gameObject);
	}

	void Update()
	{
		text_raw.text = $"raw: {raw.Float}";
		text_val.text = $"val: {val.Float}";
	}

	void SetAsRandom()
	{
		raw = new F32(UnityEngine.Random.Range(0.0f, 100.0f));
		val = new F32(UnityEngine.Random.Range(0.0f, 100.0f));
	}
}
