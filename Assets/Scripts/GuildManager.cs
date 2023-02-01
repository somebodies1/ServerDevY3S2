using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.GroupsModels;
using TMPro;

public class GuildManager : MonoBehaviour
{
    public PlayFabUserMgTMP playFabManager;

    [SerializeField] TMP_InputField createGroupInput, deleteGroupInput;
    [SerializeField] TextMeshProUGUI guildList;

    private void Start()
    {
        OnButtonListGroups();
    }

    public void UpdateGuildList()
    {
        OnButtonListGroups();
    }

    // A local cache of some bits of PlayFab data
    // This cache pretty much only serves this example , and assumes that entities are uniquely identifiable by EntityId alone, which isn't technically true. Your data cache will have to be better.
    public readonly HashSet<KeyValuePair<string, string>> EntityGroupPairs = new HashSet<KeyValuePair<string, string>>();
    public readonly Dictionary<string, string> GroupNameById = new Dictionary<string, string>();

    public static EntityKey EntityKeyMaker(string entityId)
    {
        return new EntityKey { Id = entityId};
    }

    public static EntityKey EntityKeyMakerWithType(string entityId, string entityType)
    {
        return new EntityKey { Id = entityId , Type = entityType };
    }

    private EntityKey GetEntityKey()
    {
        return EntityKeyMakerWithType(playFabManager.GetEntityKeyID(), playFabManager.GetEntityKeyType());
    }

    private void OnSharedError(PlayFab.PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void OnButtonListGroups()
    {
        ListGroups(GetEntityKey());
    }

    public void ListGroups(EntityKey entityKey)
    {
        var request = new ListMembershipRequest { Entity = entityKey };
        PlayFabGroupsAPI.ListMembership(request, OnListGroups, OnSharedError);
    }
    private void OnListGroups(ListMembershipResponse response)
    {
        var prevRequest = (ListMembershipRequest)response.Request;
        foreach (var pair in response.Groups)
        {
            GroupNameById[pair.Group.Id] = pair.GroupName;
            EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, pair.Group.Id));
            //Debug.Log("GroupID: " + pair.Group.Id);
        }

        guildList.text = "Guilds: \n";

        for (int i = 0; i < response.Groups.Count; ++i)
        {
            Debug.Log("GroupName: " + response.Groups[i].GroupName);
            guildList.text += response.Groups[i].GroupName + "\n";
        }
    }

    public void OnButtonCreateGroup()
    {
        CreateGroup(createGroupInput.text, GetEntityKey());
    }

    public void CreateGroup(string groupName, EntityKey entityKey)
    {
        // A player-controlled entity creates a new group
        var request = new CreateGroupRequest { GroupName = groupName, Entity = entityKey };
        PlayFabGroupsAPI.CreateGroup(request, OnCreateGroup, OnSharedError);
    }

    private void OnCreateGroup(CreateGroupResponse response)
    {
        Debug.Log("Group Created: " + response.GroupName + " - " + response.Group.Id);

        var prevRequest = (CreateGroupRequest)response.Request;
        EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, response.Group.Id));
        GroupNameById[response.Group.Id] = response.GroupName;

        UpdateGuildList();
    }

    public void OnButtonDeleteGroup()
    {
        DeleteGroupByName(deleteGroupInput.text);
    }

    public void DeleteGroupByName(string _groupName)
    {
        PlayFabGroupsAPI.ListMembership(
            new ListMembershipRequest { Entity = GetEntityKey() }, 
            result => {
                DeleteGroupByNameCheck(result.Groups, _groupName);
            }, 
            OnSharedError);
    }

    public void DeleteGroupByNameCheck(List<GroupWithRoles> _groups, string _groupName)
    {
        for (int i = 0; i < _groups.Count; ++i)
        {
            if (_groups[i].GroupName == _groupName)
            {
                DeleteGroup(_groups[i].Group.Id);
            }
        }
    }

    public void DeleteGroup(string groupId)
    {
        // A title, or player-controlled entity with authority to do so, decides to destroy an existing group
        var request = new DeleteGroupRequest { Group = EntityKeyMaker(groupId) };
        PlayFabGroupsAPI.DeleteGroup(request, OnDeleteGroup, OnSharedError);
    }

    private void OnDeleteGroup(EmptyResponse response)
    {
        var prevRequest = (DeleteGroupRequest)response.Request;
        Debug.Log("Group Deleted: " + prevRequest.Group.Id);

        var temp = new HashSet<KeyValuePair<string, string>>();
        foreach (var each in EntityGroupPairs)
            if (each.Value != prevRequest.Group.Id)
                temp.Add(each);
        EntityGroupPairs.IntersectWith(temp);
        GroupNameById.Remove(prevRequest.Group.Id);

        UpdateGuildList();
    }

    public void InviteToGroup(string groupId, EntityKey entityKey)
    {
        // A player-controlled entity invites another player-controlled entity to an existing group
        var request = new InviteToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
        PlayFabGroupsAPI.InviteToGroup(request, OnInvite, OnSharedError);
    }
    public void OnInvite(InviteToGroupResponse response)
    {
        var prevRequest = (InviteToGroupRequest)response.Request;

        // Presumably, this would be part of a separate process where the recipient reviews and accepts the request
        var request = new AcceptGroupInvitationRequest { Group = EntityKeyMaker(prevRequest.Group.Id), Entity = prevRequest.Entity };
        PlayFabGroupsAPI.AcceptGroupInvitation(request, OnAcceptInvite, OnSharedError);
    }
    public void OnAcceptInvite(EmptyResponse response)
    {
        var prevRequest = (AcceptGroupInvitationRequest)response.Request;
        Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
        EntityGroupPairs.Add(new KeyValuePair<string, string>(prevRequest.Entity.Id, prevRequest.Group.Id));
    }

    public void ApplyToGroup(string groupId, EntityKey entityKey)
    {
        // A player-controlled entity applies to join an existing group (of which they are not already a member)
        var request = new ApplyToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
        PlayFabGroupsAPI.ApplyToGroup(request, OnApply, OnSharedError);
    }
    public void OnApply(ApplyToGroupResponse response)
    {
        var prevRequest = (ApplyToGroupRequest)response.Request;

        // Presumably, this would be part of a separate process where the recipient reviews and accepts the request
        var request = new AcceptGroupApplicationRequest { Group = prevRequest.Group, Entity = prevRequest.Entity };
        PlayFabGroupsAPI.AcceptGroupApplication(request, OnAcceptApplication, OnSharedError);
    }
    public void OnAcceptApplication(EmptyResponse response)
    {
        var prevRequest = (AcceptGroupApplicationRequest)response.Request;
        Debug.Log("Entity Added to Group: " + prevRequest.Entity.Id + " to " + prevRequest.Group.Id);
    }
    public void KickMember(string groupId, EntityKey entityKey)
    {
        var request = new RemoveMembersRequest { Group = EntityKeyMaker(groupId), Members = new List<EntityKey> { entityKey } };
        PlayFabGroupsAPI.RemoveMembers(request, OnKickMembers, OnSharedError);
    }
    private void OnKickMembers(EmptyResponse response)
    {
        var prevRequest = (RemoveMembersRequest)response.Request;

        Debug.Log("Entity kicked from Group: " + prevRequest.Members[0].Id + " to " + prevRequest.Group.Id);
        EntityGroupPairs.Remove(new KeyValuePair<string, string>(prevRequest.Members[0].Id, prevRequest.Group.Id));
    }
}
