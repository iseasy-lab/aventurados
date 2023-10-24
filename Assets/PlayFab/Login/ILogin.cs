using System;
using PlayFab.ClientModels;

namespace PlayFab.Login {
    public interface ILogin {
        void Login(GetPlayerCombinedInfoRequestParams loginInfoParams, Action<LoginResult> loginSuccess, Action<PlayFabError> loginFailure, object loginParams);
    }
}
