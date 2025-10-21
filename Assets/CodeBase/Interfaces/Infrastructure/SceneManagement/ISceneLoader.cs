using Cysharp.Threading.Tasks;

namespace CodeBase.Interfaces.Infrastructure.SceneManagement
{
    public interface ISceneLoader
    {
        UniTask Load(string nextScene);
    }
}