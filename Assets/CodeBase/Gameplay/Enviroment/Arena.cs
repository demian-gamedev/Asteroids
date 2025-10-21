using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Gameplay.Services.Providers;
using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Enviroment
{
    public class Arena : ITickable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly PlayerProvider _playerProvider;
        
        public Vector2 Size { get; private set; }
        public Vector2 Extends => Size / 2;
        public Vector2 Center => Extends;
        
        private List<IArenaMember> _members = new List<IArenaMember>();
        private Dictionary<IArenaMember, bool> _canBeTeleportedDictionary;
        private IArenaMember _coreMember;

        public Arena(IStaticDataService staticDataService, PlayerProvider playerProvider)
        {
            _staticDataService = staticDataService;
            _playerProvider = playerProvider;
            _canBeTeleportedDictionary = new Dictionary<IArenaMember, bool>();
        }

        public void Initialize()
        {
            Size = _staticDataService.ForMap().Size;
            _coreMember = _playerProvider.PlayerPresentation;
            _canBeTeleportedDictionary[_coreMember] = true;
        }
        public void Tick()
        {
            HandleMembers();
        }

        private void HandleMembers()
        {
            if ((Object)_coreMember != null)
            {
                HandleMember(_coreMember);
                
                for (int i=_members.Count-1; i>=0; i--)
                {
                    UnityEngine.Object memberObject = _members[i] as Object;
                    if (memberObject == null)
                        _members.RemoveAt(i);
                    else
                    {
                        HandleMember(_members[i]);
                    }
                }
            }
        }

        private void HandleMember(IArenaMember member)
        {
            if (_canBeTeleportedDictionary[member])
            {
                TeleportIfOutsideArena(member.TransformData);
            }
            if (!_canBeTeleportedDictionary[member])
            {
                if (IsInsideArenaBounds(member.TransformData))
                {
                    _canBeTeleportedDictionary[member] = true;
                }
            }
            
            member.transform.position = GetViewPosition(member.TransformData);
        }

        private void TeleportIfOutsideArena(TransformData transformData)
        {
            if (transformData.Position.x < 0) transformData.Position.x += Size.x;
            else if (transformData.Position.x > Size.x) transformData.Position.x -= Size.x;
            
            if (transformData.Position.y < 0) transformData.Position.y += Size.y;
            else if (transformData.Position.y > Size.y) transformData.Position.y -= Size.y;
        }

        private bool IsInsideArenaBounds(TransformData transformData)
        {
            if (transformData.Position.x < 0) return false;
            if (transformData.Position.x > Size.x) return false;

            if (transformData.Position.y < 0) return false;
            if (transformData.Position.y > Size.y) return false;
            return true;
        }

        public void RegisterMember(IArenaMember arenaMember)
        {
            _canBeTeleportedDictionary[arenaMember] = false;
            _members.Add(arenaMember);
        }

        private Vector2 GetViewPosition(TransformData data)
        {
            Vector2 res = new Vector2(data.Position.x, data.Position.y);
            
            if (Mathf.Abs(data.Position.x - _coreMember.TransformData.Position.x) > Extends.x)
            {
                if (res.x - Extends.x < 0)
                {
                    res.x += Size.x;
                }else if (res.x + Extends.x > Size.x)
                {
                    res.x -= Size.x;
                }
            }
            if (Mathf.Abs(data.Position.y - _coreMember.TransformData.Position.y) > Extends.y)
            {
                if (res.y - Extends.y < 0)
                {
                    res.y += Size.y;
                }else if (res.y + Extends.y > Size.y)
                {
                    res.y -= Size.y;
                }
            }
            
            return res;
        }

        public void RemoveMember(IArenaMember member)
        {
            _canBeTeleportedDictionary.Remove(member);
            _members.Remove(member);
        }
    }
}