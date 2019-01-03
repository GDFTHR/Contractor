using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using BepInEx;
using Manager;
using UnityEngine;

namespace NamaInsert
{
    [BepInProcess("Koikatu")]
    [BepInPlugin("Nama_insert", "Nama_insert", "1.0")]
	public class NamaInsert : BaseUnityPlugin
	{
        private const string HScene = "Nama Insert";

        [Category(HScene)]
        [DisplayName("!Reset Nama Insert")]
        public static ConfigWrapper<bool> CountNamaInsert { get; set; }

        private Game _gameMgr;
        private Scene _sceneMgr;

        public NamaInsert()
        {
            CountNamaInsert = new ConfigWrapper<bool>("countNamaInsert", this, true);

        }

        // Start as false to prevent firing after loading
        private bool _inNightMenu, _firstNightMenu = true;

        private void OnNightStarted()
        {
            if (CountNamaInsert.Value)
            {
                foreach (var heroine in _gameMgr.HeroineList)
                if (heroine.countNamaInsert >= 5)
                {
                    heroine.countNamaInsert = Math.Max(4, heroine.countNamaInsert - 50);
                }
            }
        }

        public void Start()
        {
            _gameMgr = Game.Instance;
            _sceneMgr = Scene.Instance;

            StartCoroutine(SlowUpdate());
        }

        private IEnumerator SlowUpdate()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);

                if (!_gameMgr.saveData.isOpening && !_sceneMgr.IsNowLoading)
                {
                    if (_sceneMgr.NowSceneNames.Any(x => x.Equals("NightMenu", StringComparison.Ordinal)))
                    {
                        if (!_inNightMenu && !_firstNightMenu)
                            OnNightStarted();
                        _inNightMenu = true;
                        _firstNightMenu = false;
                    }
                    else
                    {
                        _inNightMenu = false;
                    }
                }
            }
        }

        /*

        private void Update()
        {

        }
        private void OnGUI()
        {

        }*/
    }
}