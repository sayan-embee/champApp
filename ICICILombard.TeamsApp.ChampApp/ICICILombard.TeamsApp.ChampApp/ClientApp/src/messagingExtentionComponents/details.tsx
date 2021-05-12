import React from 'react';
import { Form, FormTextArea, FormDropdown, Text, Flex, Button, Card, Input, CardBody } from "@fluentui/react-northstar";
import { ArrowLeftIcon, SearchIcon, CloseIcon } from '@fluentui/react-icons-northstar';

import "./../styles.scss"

// import { getViswasBehaviourAPI } from '../apis/getViswasBehaviourApi'
import { getTeamMembersAPI,getChatMembersAPI } from "../apis/getTeamMembers";
import { getAppSettingAPI } from "../apis/settingApi"
import { getViswasBehaviourAPI } from './../apis/ViswasBehaviourApi'
import * as microsoftTeams from "@microsoft/teams-js";




interface IDetailsProps {
  location?: any;
  history?: any
}

interface IDetailsState {
  badgeName?: any;
  badgeImage?: any;
  badgeColor?: any;
  behaviourInputItem?: any;
  formItem?: any;
  behaviours?: any;
  reason?: any;
  allData?: any;
  files?: any;
  showList?: any;
  selectedEmployeeName?: any;
  searchItem?: any;
  multipleSelection?: any;
  teamMembers?: any;
  badgeId?: any;
  userId?: any;
  UPN?: any;
  awardedByName?: any;
  groupId?: any;
  token?: any;
  chatId?:any;
  personalChat?:any;
  viswasBehaviourRequire?: any;
  IsGroupSingle?: any;
  IsGroupMultiple?: any;
  IsChannelSingle?: any;
  IsChannelMultiple?: any;
  getData?: any;
  behaviourId?: any;
  behaviourList?: any;
  behavioursId?:any;
  channelName?:any;
  Recipents?:any;
  authToken?:any
}

class Details extends React.Component<IDetailsProps, IDetailsState> {

  constructor(props: IDetailsProps) {
    super(props);
    this.state = {
      formItem: [
        { text: "Vishvas Behaviours", placeholder: "Select Option", dropdown: true, key: "behaviour" },
        { text: "Reason For Nomination(25 char min)", placeholder: "#Kudos #Silver.", dropdown: false, key: "reason" }
      ],
      allData: [],
      selectedEmployeeName: [],
      Recipents:[],
      multipleSelection: true
    };
  }




  componentDidMount() {
    if (this.props.location.state) {
      this.setState({
        badgeImage: this.props.location.state.data && this.props.location.state.data.badgeImage,
        badgeName: this.props.location.state.data && this.props.location.state.data.badgeName,
        badgeColor: this.props.location.state.data && this.props.location.state.data.badgeColor,
        badgeId: this.props.location.state.data && this.props.location.state.data.badgeId,
        groupId: this.props.location.state.groupId && this.props.location.state.groupId,
        chatId:this.props.location.state.chatId && this.props.location.state.chatId,
        personalChat:this.props.location.state.personalChat && this.props.location.state.personalChat,
        channelName:this.props.location.state.channelName && this.props.location.state.channelName,
        token: this.props.location.state.token && this.props.location.state.token,
        userId:this.props.location.state.userId && this.props.location.state.userId,
        authToken:this.props.location.state.authToken && this.props.location.state.authToken,
        UPN:this.props.location.state.UPN && this.props.location.state.UPN,
        reason: this.props.location.state.data && this.props.location.state.data.reason ? this.props.location.state.data.reason : null,
        behaviours: this.props.location.state.data && this.props.location.state.data.behaviours ? this.props.location.state.data.behaviours : null,
        selectedEmployeeName:this.props.location.state.data && this.props.location.state.data.name ? this.props.location.state.data.name:this.state.selectedEmployeeName
      }, () => {
        this.getTeamMembers()
        this.getViswasBehaviour()
         this.getAppSetting()
      })
    }
    console.log("details log", this.props.location.state)

  }

  getAppSetting(){
    getAppSettingAPI().then((res)=>{
      console.log("setting",res);
      this.setState({
        IsGroupSingle: res.data.isGroupSingle,
        IsGroupMultiple: res.data.isGroupMultiple,
        IsChannelSingle: res.data.isChannelSingle,
        IsChannelMultiple: res.data.isChannelMultiple,
        viswasBehaviourRequire: (res.data.isBehaviourRequired===1)?true:false,
        getData: true,
        multipleSelection: (res.data.isGroupSingle === 1) ? false : true
      })
    })
  }

  getViswasBehaviour = () => {
    const data = {
      // "IsActive": 1,
      "BehaviourId": 0
  }
  getViswasBehaviourAPI(data).then((res:any) => {
      console.log("api visws get", res.data);
      let list=res.data
      let result = res.data.filter((e:any)=>e.isActive===1).map((a: any) => a.behaviourName)
      console.log("behaviour input item response", result);
      this.setState({
        behaviourInputItem: result,
        behaviourList: list
      },()=>{
        console.log("list",this.state.behaviourList);
        
      })
    })

  
    // getViswasBehaviourAPI(this.state.token).then((res) => {
    //   let list=res.data
    //   let result = res.data.map((a: any) => a.title)
    //   console.log("behaviour input item response", result);
    //   this.setState({
    //     behaviourInputItem: result,
    //     behaviourList: list
    //   },()=>{
    //     console.log("list",this.state.behaviourList);
        
    //   })
    // })
  }

  getTeamMembers = () => {
   if(this.state.personalChat){
     console.log("chat",this.state.chatId);
     console.log("token",this.state.authToken);
    getChatMembersAPI(this.state.chatId, this.state.authToken).then((res) => {
      console.log("chat member", res.data);
      this.setState({
        teamMembers: res.data
      },()=>{
        res.data.filter((e: any) => e.roles[0] === "owner").map((e:any)=>{
          this.setState({
            awardedByName:e.displayName
          })
        })
      })
    })
   }
   else{
    getTeamMembersAPI(this.state.groupId, this.state.token).then((res) => {
      console.log("team member", res.data);
      this.setState({
        teamMembers: res.data
      },()=>{
        res.data.filter((e: any) => e.roles[0] === "owner").map((e:any)=>{
          this.setState({
            awardedByName:e.displayName
          })
        })
      })
    })
   }
    
  }

  back() {
    this.props.history.push(`/badge?token=${this.state.token}`)
  }


  check = (data: any, key: any) => {
    if (key === "behaviour") {
       this.state.behaviourList.filter((e: any) => e.behaviourName === data).map((e:any)=>{
        this.setState({
          behavioursId:e.behaviourId
        })

       })
      this.setState({
        "behaviours": data
      })
    }
    else if (key === "reason") {
      this.setState({
        "reason": data.target.value
      })
    }
  }

  search(event: any) {
    this.setState({
      searchItem: event.target.value
    }, () => {
      if (this.state.searchItem) {
        this.searchFunction(this.state.searchItem.toLowerCase())
      }
    })
  }


  searchFunction(name: any) {
    // console.log("team members", this.state.teamMembers);
    const dataSet =
      this.state.teamMembers.filter((item: any) => {
        let isMatch = false;
        if (item.displayName.toLowerCase().indexOf(name.toLowerCase()) >= 0) {
          isMatch = true;
        }
        return isMatch;
      })
    this.setState({
      files: dataSet,
      showList: true,
    });
  }


  selectEmployeeNameFunction(ele: any) {
    console.log("check")
    this.setState({
      selectedEmployeeName: [...this.state.selectedEmployeeName, { "name": ele.displayName, "email": ele.email, "userId": ele.userId }],
      Recipents:[...this.state.Recipents,{"RecipentName":ele.displayName,"RecipentEmail":ele.email}],
      searchItem: null
    })
  }


  cancelEmployee(index: any) {
    var array = [...this.state.selectedEmployeeName];
    if (index !== -1) {
      array.splice(index, 1);
      this.setState({ selectedEmployeeName: array });
    }
  }


  previewBtnFunction() {
    if (this.state.selectedEmployeeName.length > 0 && this.state.badgeImage && this.state.badgeName  && this.state.reason) {
      this.setState({
        allData: [...this.state.allData, { "name": this.state.selectedEmployeeName, "badgeImage": this.state.badgeImage, "badgeName": this.state.badgeName, "badgeColor": this.state.badgeColor, "badgeId": this.state.badgeId, "behaviours": this.state.behaviours, "reason": this.state.reason,"behavioursId":this.state.behavioursId,"Recipents":this.state.Recipents }]
      }, () => {
        this.props.history.push({
          pathname: '/preview', state: { data: this.state.allData, token: this.state.token, groupId: this.state.groupId, userId:this.state.userId, UPN:this.state.UPN, awardedByName:this.state.awardedByName, channelName:this.state.channelName }
        })
      })
    }
    else {
      alert("Please select all the field")
    }
  }



  render() {
    return (
      <div>

        <Card fluid className="containerCard">
          <CardBody>
            <Flex><Text>Batch</Text></Flex>
            <Flex space="between">
              <Card selected centered styles={{
                width: '165px',
                borderRadius: '6px',
                marginTop: '5px',
                backgroundColor: this.state.badgeColor,
                ':hover': {
                  backgroundColor: `${this.state.badgeColor} !important`,
                },
              }}>
                {this.state.badgeImage && <img src={this.state.badgeImage} />}
                {this.state.badgeName && <text className="badgeText">{this.state.badgeName}</text>}
              </Card>


              <div style={{ width: "55%" }}>
                {this.state.teamMembers && <div>
                  <Input value={this.state.searchItem} disabled={(!this.state.multipleSelection && (this.state.selectedEmployeeName.length > 0)) ? true : false} required fluid clearable icon={<SearchIcon style={{ height: "15px", width: "15px" }} />} placeholder="Type here the name you want to select..." onChange={(e) => this.search(e)} />
                  {(this.state.files && (this.state.searchItem)) && <div className='searchList'>
                    {this.state.files.filter((e: any) => e.roles[0] !== "owner").map((ele: any, i: any) =>
                      <div key={i} >
                        <div className='searchResultList' onClick={() => this.selectEmployeeNameFunction(ele)}>
                          <Text className="searchResultListEmployeeName"> {ele.displayName} </Text>
                        </div>
                      </div>
                    )}
                  </div>}
                </div>}
                {this.state.selectedEmployeeName && <div>
                  {this.state.selectedEmployeeName.length > 0 && <div style={{ marginTop: "20px" }}>

                  {/* <div style={{ marginBottom: "10px" }}>
                      <Text content={"Employee Name"} size="large" />
                    </div> */}
                  {this.state.selectedEmployeeName.map((e: any, i: any) => {
                    return <Flex column styles={{ marginBottom: "10px" }}>
                      <Flex space="between" styles={{ marginBottom: "5px" }} >
                        <Text content={e.name} size="medium" />
                        <Button circular icon={<CloseIcon size="small" />} iconOnly title="Close" size="small" onClick={() => this.cancelEmployee(i)} />
                      </Flex>

                    </Flex>
                  })}

                </div>}
                </div>}

              </div>



            </Flex>

            <Form styles={{ paddingTop: '25px' }}>
              {this.state.formItem.map((e: any) => {
                return <div>
                  {e.dropdown ? (this.state.behaviourInputItem && this.state.viswasBehaviourRequire) && <FormDropdown fluid
                    label={{ content: e.text }}
                    items={this.state.behaviourInputItem}
                    // search={true}
                    onChange={(event, { value }) => this.check(value, e.key)}
                    placeholder={e.placeholder}
                    value={this.state.behaviours}
                  /> :
                    <FormTextArea label={ e.text } value={this.state.reason && this.state.reason} onChange={(event) => this.check(event, e.key)} fluid resize="vertical" placeholder={e.placeholder}  className="textAreaStyles" />}
                </div>

              })}


            </Form>
          </CardBody>


        </Card>

        <div className="margin20">
          <Flex space="between">
            <Button onClick={() => this.back()} icon={<ArrowLeftIcon />} text content="Back" />
            <Flex gap="gap.small">
              <Button content="Cancel" onClick={() => microsoftTeams.tasks.submitTask()}/>
              <Button primary onClick={() => this.previewBtnFunction()}>Preview</Button>
            </Flex>
          </Flex>
        </div>

      </div>
    );
  }
}



export default Details;

