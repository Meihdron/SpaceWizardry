using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.DataObjects
{
    public class character : IDataObject
    {
        private const bool _isPersistable = true;
        
        private int _CharacterID ;              
        private string _name;
        private string _description;
        private int _corporation_id;
        private int _alliance_id;
        private DateTime _birthday;
        //private string _gender;
        //private int _race_id;
        //private int _bloodline_id;
        //private int _ancestry_id;
        private decimal _security_status;
        private int _faction_id;            //faction id the pilot is fighting for




private ESI_Token _token;

        public bool isPersistable
        {
            get { return _isPersistable; }
        }


        public void saveObject() { }
        public void loadObject() { }


    }
}


//GET /characters/{character_id}/
//        {
//  "application/json": {
//    "corporation_id": 109299958,
//    "birthday": "2015-03-24T11:37:00Z",
//    "name": "CCP Bartender",
//    "gender": "male",
//    "race_id": 2,
//    "description": "",
//    "bloodline_id": 3,
//    "ancestry_id": 19
//  }
//}



//GET /characters/{character_id}/corporationhistory
//{
//  "application/json": [
//    {
//      "start_date": "2016-06-26T20:00:00Z",
//      "corporation_id": 90000001,
//      "is_deleted": true,
//      "record_id": 500
//    },
//    {
//      "start_date": "2016-07-26T20:00:00Z",
//      "corporation_id": 90000002,
//      "record_id": 501
//    }
//  ]
//}



//GET /characters/{character_id}/chat_channels/
//        {
//  "application/json": [
//    {
//      "channel_id": -69329950,
//      "name": "Players' Haven",
//      "owner_id": 95578451,
//      "comparison_key": "players'haven",
//      "has_password": false,
//      "motd": "<b>Feed pineapples to the cats!</b>",
//      "allowed": [],
//      "operators": [],
//      "blocked": [],
//      "muted": []
//    }
//  ]
//}


//GET /characters/{character_id}/standings/
//    {
//  "application/json": [
//    {
//      "from_id": 3009841,
//      "from_type": "agent",
//      "standing": 0.1
//    },
//    {
//      "from_id": 1000061,
//      "from_type": "npc_corp",
//      "standing": 0
//    },
//    {
//      "from_id": 500003,
//      "from_type": "faction",
//      "standing": -1
//    }
//  ]
//}



//GET /characters/{character_id}/fatigue/
//{
//  "application/json": {
//    "last_jump_date": "2017-07-05T15:47:00Z",
//    "jump_fatigue_expire_date": "2017-07-06T15:47:00Z",
//    "last_update_date": "2017-07-05T15:42:00Z"
//  }
//}


//GET /characters/{character_id}/roles/
//    {
//  "application/json": {
//    "roles": [
//      "Director",
//      "Station_Manager"
//    ]
//  }
//}


//GET /characters/{character_id}/stats/
//{
//  "application/json": [
//    {
//      "year": 2014,
//      "character": {
//        "days_of_activity": 365,
//        "minutes": 1000000,
//        "sessions_started": 500
//      },
//      "combat": {
//        "kills_low_sec": 42
//      }
//    },
//    {
//      "year": 2015,
//      "character": {
//        "days_of_activity": 365,
//        "minutes": 1000000,
//        "sessions_started": 500
//      },
//      "combat": {
//        "kills_null_sec": 1337
//      }
//    }
//  ]
//}