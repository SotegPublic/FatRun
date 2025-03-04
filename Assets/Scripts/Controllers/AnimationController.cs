using Runner.Core;
using System;
using UnityEngine;

namespace Runner.Animation
{
    public class AnimationController : MonoBehaviour, IPausable
    {
        private Animator _animator;
        private AnimationsEventManager _animationsEventManager;
        private PlayerGameModel _playerGameModel;

        private bool _isStay;
        private bool _isFightIdle;
        private bool _isRun;
        private bool _isJump;

        public AnimationsEventManager AnimationsEventManager => _animationsEventManager;

        public void InitController(PlayerGameModel playerGameModel)
        {
            _playerGameModel = playerGameModel;
            _animator = GetComponentInChildren<Animator>();
            _animationsEventManager = GetComponentInChildren<AnimationsEventManager>();

            SetAnimationsID(playerGameModel);
            _animator.SetFloat("SpeedModifier", playerGameModel.SpeedModifier);
        }

        public void ActivatePause(bool isPaused)
        {
            if(isPaused)
            {
                _animator.speed = 0;
            }
            else
            {
                _animator.speed = 1;
                _animator.SetFloat("SpeedModifier", _playerGameModel.SpeedModifier);
            }
        }
        
        public void ActivateRJump()
        {
            if (_isJump) return;

            _animator.SetInteger(AnimationConstants.JUMP_PARAMETER_NAME, AnimationConstants.RIGHT_JUMP_ID);
            _animator.SetTrigger("Jump");
            _animator.ResetTrigger("Run");
            _animator.ResetTrigger("Stay");
            _isJump = true;
            _isRun = false;
            _isStay = false;
        }

        public void ActivateLJump()
        {
            if (_isJump) return;

            _animator.SetInteger(AnimationConstants.JUMP_PARAMETER_NAME, AnimationConstants.LEFT_JUMP_ID);
            _animator.SetTrigger("Jump");
            _animator.ResetTrigger("Run");
            _animator.ResetTrigger("Stay");
            _isJump = true;
            _isRun = false;
            _isStay = false;
        }

        public void ActivateIdle()
        {
            if (_isStay) return;

            _animator.SetTrigger("Stay");
            _animator.ResetTrigger("Run");
            _animator.ResetTrigger("Jump");

            _isJump = false;
            _isRun = false;
            _isStay = true;
        }

        public void ActivateRun()
        {
            if (_isRun) return;

            _animator.SetTrigger("Run");
            _animator.ResetTrigger("Jump");
            _animator.ResetTrigger("Stay");

            _isJump = false;
            _isRun = true;
            _isStay = false;
        }

        public void ActivateKick()
        {
            _isFightIdle = false;
            _animator.SetTrigger("Kick");
            _animator.ResetTrigger("FightIdle");
        }

        public void ActivateFightIdle()
        {
            if(_isFightIdle) return;

            _animator.SetTrigger("FightIdle");
            _animator.ResetTrigger("Stay");
            _animator.ResetTrigger("Kick");
            _isFightIdle = true;
        }

        public void ActivateDance()
        {
            _animator.SetTrigger("Dance");
            _animator.ResetTrigger("Kick");
        }

        public void ActivateSad()
        {
            _animator.SetTrigger("Sad");
            _animator.ResetTrigger("Kick");
        }

        public void SetRunID(int id)
        {
            _animator.SetInteger(AnimationConstants.RUN_PARAMETER_NAME, id);
        }
        public void SetKickID(int id)
        {
            _animator.SetInteger(AnimationConstants.KICK_PARAMETER_NAME, id);
        }

        private void SetAnimationsID(PlayerGameModel playerGameModel)
        {
            _animator.SetInteger(AnimationConstants.RUN_PARAMETER_NAME, playerGameModel.RunAnimationID);
            _animator.SetInteger(AnimationConstants.KICK_PARAMETER_NAME, playerGameModel.KickAnimationID);
            _animator.SetInteger(AnimationConstants.DANCE_PARAMETER_NAME, playerGameModel.DanceAnimationID);
        }

    }
}