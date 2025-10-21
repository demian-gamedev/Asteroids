using System;
using CodeBase.Common.Attributes;
using CodeBase.Gameplay.Services.Providers;
using CodeBase.Interfaces.UI;
using UniRx;
using Zenject;

namespace CodeBase.Gameplay.ViewModels
{
    public class PlayerViewModel : IViewModel, IInitializable, IDisposable
    {
        [Data("Position")]
        public ReactiveProperty<string> Position = new();
        [Data("Rotation")]
        public ReactiveProperty<string> Rotation = new();
        [Data("Speed")]
        public ReactiveProperty<string> Speed = new();
        [Data("ChargesLeft")]
        public ReactiveProperty<string> LaserChargesLeft = new();
        [Data("ChargeReload")]
        public ReactiveProperty<string> LaserChargeReload = new();
    
        private readonly PlayerProvider _playerProvider;
        private readonly CompositeDisposable _disposables = new();

        public PlayerViewModel(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public void Initialize()
        {
            ObservePosition();
            ObserveRotation();
            ObserveSpeed();
            ObserveLaser();
        }

        private void ObserveLaser()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    LaserChargesLeft.Value = "Charges: " + _playerProvider.PlayerPresentation.LaserCharges;
                })
                .AddTo(_disposables);
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    LaserChargeReload.Value = "Reload: " + Math.Round(
                        _playerProvider.PlayerPresentation.LaserChargeReload, 2);
                })
                .AddTo(_disposables);
        }

        private void ObserveSpeed()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Speed.Value = "Speed: " + Math.Round(_playerProvider.PlayerPresentation.Speed, 1);
                })
                .AddTo(_disposables);
        }

        private void ObserveRotation()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Rotation.Value = "Rotation: " + Math.Round(
                        _playerProvider.PlayerPresentation.TransformData.Rotation);
                })
                .AddTo(_disposables);
        }

        private void ObservePosition()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Position.Value = "Position: " + _playerProvider.PlayerPresentation.TransformData.Position.ToString();
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}