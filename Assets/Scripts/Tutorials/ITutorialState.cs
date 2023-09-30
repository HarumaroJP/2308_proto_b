// --------------------------------------------------------- 
// ITutorialState.cs 
// �`���[�g���A���̃X�e�[�g�p�C���^�[�t�F�[�X
// 
// CreateDay: 2023/09/09
// Creator  : Ushimaru
// --------------------------------------------------------- 
using UniRx;

public interface ITutorialState
{
	//	�X�e�[�g�̏���������
	public void OnEnterState();
	//	�X�e�[�g�𔲂���ۂ̏���
	public void OnExitState();

	//	�X�e�[�g�̍X�V����
	public void UpdateState();

	//	�\������e�L�X�g���擾����
	public string TutorialText { get; }
}