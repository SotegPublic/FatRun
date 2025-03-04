using Runner.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.BonusSystem
{
    public class BonusesPoolController : MonoBehaviour, IGameSystem, IDisposable
    {
        [SerializeField] private GameObject _bonusPrefab;
        [SerializeField] private GameObject _hurdlePrefab;
        [SerializeField] private Transform _poolTransform;
        [SerializeField] private int _poolCapacity;

        private BonusFactory<BonusController> _bonusFactory;
        private BonusFactory<HurdleController> _hurdleFactory;
        private List<BonusController> _bonusesList;
        private List<HurdleController> _hurdlesList;


        public void InitPool()
        {
            _bonusFactory = new BonusFactory<BonusController>(_bonusPrefab);
            _hurdleFactory = new BonusFactory<HurdleController>(_hurdlePrefab);

            _bonusesList = new List<BonusController>(_poolCapacity);
            _hurdlesList = new List<HurdleController>(_poolCapacity);
            FillPool();
        }

        private void FillPool()
        {
            for (int i = 0; i < _poolCapacity; i++)
            {
                var bonus = _bonusFactory.CreateBonus(_poolTransform);
                _bonusesList.Add(bonus);

                var hurdle = _hurdleFactory.CreateBonus(_poolTransform);
                _hurdlesList.Add(hurdle);
            }
        }

        public BonusController GetBonus()
        {
            BonusController bonus = null;

            if(_bonusesList.Count == 0)
            {
                bonus = _bonusFactory.CreateBonus( _poolTransform);
            }
            else
            {
                bonus = _bonusesList[0];
                _bonusesList.Remove(bonus);
            }

            return bonus;
        }

        public HurdleController GetHurdle()
        {
            HurdleController hurdle = null;

            if (_hurdlesList.Count == 0)
            {
                hurdle = _hurdleFactory.CreateBonus(_poolTransform);
            }
            else
            {
                hurdle = _hurdlesList[0];
                _hurdlesList.Remove(hurdle);
            }

            return hurdle;
        }

        public void ReturnBonus(BonusController bonus)
        {
            _bonusesList.Add(bonus);
            bonus.transform.parent = _poolTransform;
            bonus.transform.localPosition = Vector3.zero;
        }

        public void ReturnHurdle(HurdleController hurdle)
        {
            _hurdlesList.Add(hurdle);
            hurdle.transform.parent = _poolTransform;
            hurdle.transform.localPosition = Vector3.zero;
        }

        public void Dispose()
        {
            _bonusesList.Clear();
            _hurdlesList.Clear();
        }
    }
}