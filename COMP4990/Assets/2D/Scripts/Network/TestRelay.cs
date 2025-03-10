using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour
{

    private async void Start(){
       
        await UnityServices.InitializeAsync();
        //code under here will not run right away until service gives ok
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    } 
    
    public async void CreateRelay(){
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode =  await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            if(NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started With Relay");
            }else{
                Debug.LogError("Host Faild with relay");
            }
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinRelay (string joinCode){
        try{
            Debug.Log("Joining Relay With " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        }catch (RelayServiceException e){
            Debug.Log(e);
        }
    }
}
