// --------------------------------------------------------- 
// ITutorialState.cs 
// チュートリアルのステート用インターフェース
// 
// CreateDay: 2023/09/09
// Creator  : Ushimaru
// --------------------------------------------------------- 
using UniRx;

public interface ITutorialState
{
	//	ステートの初期化処理
	public void OnEnterState();
	//	ステートを抜ける際の処理
	public void OnExitState();

	//	ステートの更新処理
	public void UpdateState();

	//	表示するテキストを取得する
	public string TutorialText { get; }
}