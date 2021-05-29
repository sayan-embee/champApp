import React from 'react';
import { Form, FormTextArea, FormDropdown, Text, Flex, Button, Card, Input, CardBody } from "@fluentui/react-northstar";
// import { SearchIcon } from '@fluentui/react-icons-northstar';

import "./../styles.scss"

import { getTeamMembersAPI, getChatMembersAPI } from "../apis/getTeamMembers";
import { getAppSettingAPI } from "../apis/settingApi"
import { getViswasBehaviourAPI } from './../apis/ViswasBehaviourApi'
import * as microsoftTeams from "@microsoft/teams-js";


const baseUrl = window.location.origin
const backImage = baseUrl + "/images/left-arrow.svg"
const cancelImage = baseUrl + "/images/cancel.svg"

interface IDetailsProps {
  location?: any;
  history?: any
}

interface IDetailsState {
  badgeName?: any;
  badgeImage?: any;
  behaviourInputItem?: any;
  formItem?: any;
  behaviours?: any;
  reason?: any;
  invalidReason?: any;
  allData?: any;
  files?: any;
  selectedEmployeeName?: any;
  searchItem?: any;
  multipleSelection?: any;
  teamMembers?: any;
  badgeId?: any;
  userId?: any;
  UPN?: any;
  awardedByName?: any;
  awardedByImage?: any;
  groupId?: any;
  token?: any;
  chatId?: any;
  viswasBehaviourRequire?: any;
  IsGroupSingle?: any;
  IsGroupMultiple?: any;
  IsChannelSingle?: any;
  IsChannelMultiple?: any;
  behaviourList?: any;
  behavioursId?: any;
  channelName?: any;
  Recipents?: any;
  channelId?: any;
  userObjectId?: any;
  teamId?: any;
  teamName?:any;
  IsGroup?: any;
  IsTeam?: any,
  IsChat?: any,
}

class Details extends React.Component<IDetailsProps, IDetailsState> {

  constructor(props: IDetailsProps) {
    super(props);
    this.state = {
      formItem: [
        { text: "Values/Behaviors", placeholder: "Select Option", dropdown: true, key: "behaviour" },
        { text: "Reason for applause", placeholder: "Add a personalized note", dropdown: false, key: "reason" }
      ],
      allData: [],
      selectedEmployeeName: [],
      Recipents: [],
      multipleSelection: true,
      invalidReason: false
    };
  }




  componentDidMount() {
    if (this.props.location.state) {
      this.setState({
        badgeImage: this.props.location.state.data && this.props.location.state.data.badgeImage,
        badgeName: this.props.location.state.data && this.props.location.state.data.badgeName,
        badgeId: this.props.location.state.data && this.props.location.state.data.badgeId,
        groupId: this.props.location.state.groupId && this.props.location.state.groupId,
        chatId: this.props.location.state.chatId && this.props.location.state.chatId,
        channelName: this.props.location.state.channelName && this.props.location.state.channelName,
        token: this.props.location.state.token && this.props.location.state.token,
        userId: this.props.location.state.userId && this.props.location.state.userId,
        userObjectId: this.props.location.state.userObjectId && this.props.location.state.userObjectId,
        UPN: this.props.location.state.UPN && this.props.location.state.UPN,
        teamId: this.props.location.state.teamId && this.props.location.state.teamId,
        teamName: this.props.location.state.teamName && this.props.location.state.teamName,
        channelId: this.props.location.state.channelId && this.props.location.state.channelId,
        reason: this.props.location.state.data && this.props.location.state.data.reason ? this.props.location.state.data.reason : null,
        behaviours: this.props.location.state.data && this.props.location.state.data.behaviours ? this.props.location.state.data.behaviours : null,
        selectedEmployeeName: this.props.location.state.data && this.props.location.state.data.name ? this.props.location.state.data.name : this.state.selectedEmployeeName,
        Recipents: this.props.location.state.data && this.props.location.state.data.Recipents ? this.props.location.state.data.Recipents : this.state.Recipents
      }, () => {
        this.getTeamMembers()
        this.getViswasBehaviour()
        this.getAppSetting()
      })
    }
  }

  getAppSetting() {
    getAppSettingAPI().then((res) => {
      // console.log("setting", res);
      this.setState({
        IsGroupSingle: res.data.isGroupSingle,
        IsGroupMultiple: res.data.isGroupMultiple,
        IsChannelSingle: res.data.isChannelSingle,
        IsChannelMultiple: res.data.isChannelMultiple,
        viswasBehaviourRequire: (res.data.isBehaviourRequired === 1) ? true : false,
        multipleSelection: (this.state.chatId != "") ? (res.data.isGroupSingle === 1) ? false : true : (res.data.isChannelSingle === 1) ? false : true
      })
    })
  }

  getViswasBehaviour = () => {
    const data = {
      "BehaviourId": 0
    }
    getViswasBehaviourAPI(data).then((res: any) => {
      // console.log("api visws get", res.data);
      let list = res.data
      let result = res.data.filter((e: any) => e.isActive === 1).map((a: any) => a.behaviourName)
      console.log("behaviour input item response", result);
      this.setState({
        behaviourInputItem: result,
        behaviourList: list
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
    if (this.state.chatId != "") {
      getChatMembersAPI(this.state.chatId, this.state.token).then((res) => {
        // console.log("chat member", res.data);
        let member = res.data.filter((e: any) => e.upn !== this.state.UPN)
        this.setState({
          teamMembers: member
        }, () => {
          if (this.state.teamMembers.length === 1) {
            if (this.state.selectedEmployeeName && this.state.selectedEmployeeName.length === 0) {
              this.setState({
                selectedEmployeeName: [...this.state.selectedEmployeeName, { "name": this.state.teamMembers[0].displayName, "email": this.state.teamMembers[0].email, "userId": this.state.teamMembers[0].aadObjectId }],
                Recipents: [...this.state.Recipents, { "RecipentName": this.state.teamMembers[0].displayName, "RecipentEmail": this.state.teamMembers[0].email, "userId": this.state.teamMembers[0].aadObjectId }],
              })
            }
            this.setState({
              IsGroup: 0,
              IsTeam: 0,
              IsChat: 1,
            })

          }
          else {
            this.setState({
              IsGroup: 1,
              IsTeam: 0,
              IsChat: 0,
            })
          }
          res.data.filter((e: any) => e.upn === this.state.UPN).map((e: any) => {
            this.setState({
              awardedByName: e.displayName,
              awardedByImage: e.photo
            })
          })
        })
      })
    }
    else {
      getTeamMembersAPI(this.state.channelId, this.state.token).then((res) => {
        // console.log("team member", res.data);
        this.setState({
          teamMembers: res.data,
          IsGroup: 0,
          IsTeam: 1,
          IsChat: 0,
        }, () => {
          res.data.filter((e: any) => e.upn === this.state.UPN).map((e: any) => {
            this.setState({
              awardedByName: e.displayName,
              awardedByImage: e.photo
            })
          })
        })
      })
    }

  }

  back() {
    this.props.history.push(`/badge?token=${this.state.token}&badgeId=${this.state.badgeId}`)
  }


  check = (data: any, key: any) => {
    if (key === "behaviour") {
      this.state.behaviourList.filter((e: any) => e.behaviourName === data).map((e: any) => {
        this.setState({
          behavioursId: e.behaviourId,
          behaviours: data
        })
      })
    }
    else if (key === "reason") {
      // if (data.target.value.length < 25) {
      //   this.setState({
      //     invalidReason: true
      //   })
      //   console.log("string invalid", data.target.value.length);

      // }
      // else {
      this.setState({
        reason: data.target.value,
        // invalidReason: false
      })
      //   console.log("string valid", data.target.value.length);
      // }

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
    });
  }


  selectEmployeeNameFunction(ele: any) {
    const employeeMail = ele.email
    if (this.state.selectedEmployeeName.length > 0) {
      const found = this.state.selectedEmployeeName.some((e: any) => e.email === employeeMail)
      if (!found) {    
       this.setState({
          selectedEmployeeName: [...this.state.selectedEmployeeName, { "name": ele.displayName, "email": ele.email, "userId": ele.aadObjectId, "photo":ele.photo }],
          Recipents: [...this.state.Recipents, { "RecipentName": ele.displayName, "RecipentEmail": ele.email, "userId": ele.aadObjectId }],
          searchItem: null
        })

      }
      else {
        this.setState({
          searchItem: null
        })

      }

    }
    else {
      this.setState({
        selectedEmployeeName: [...this.state.selectedEmployeeName, { "name": ele.displayName, "email": ele.email, "userId": ele.aadObjectId, "photo":ele.photo }],
        Recipents: [...this.state.Recipents, { "RecipentName": ele.displayName, "RecipentEmail": ele.email, "userId": ele.aadObjectId }],
        searchItem: null
      })
    }
  }


  cancelEmployee(index: any) {
    var array = [...this.state.selectedEmployeeName];
    if (index !== -1) {
      array.splice(index, 1);
      this.setState({ selectedEmployeeName: array });
    }
  }


  previewBtnFunction() {
    this.setState({
      allData: [...this.state.allData, { "name": this.state.selectedEmployeeName, "badgeImage": this.state.badgeImage, "badgeName": this.state.badgeName,"badgeId": this.state.badgeId, "behaviours": this.state.behaviours, "reason": this.state.reason, "behavioursId": this.state.behavioursId, "Recipents": this.state.Recipents }]
    }, () => {
      this.props.history.push({
        pathname: '/preview', state: {
          data: this.state.allData,
          token: this.state.token,
          groupId: this.state.groupId,
          chatId: this.state.chatId,
          teamId: this.state.teamId,
          teamName: this.state.teamName,
          channelId: this.state.channelId,
          userObjectId: this.state.userObjectId,
          userId: this.state.userId,
          UPN: this.state.UPN,
          awardedByName: this.state.awardedByName,
          awardedByImage:this.state.awardedByImage,
          channelName: this.state.channelName,
          isGroup: this.state.IsGroup,
          isTeam: this.state.IsTeam,
          isChat: this.state.IsChat
        }
      })
    })
  }



  render() {
    return (
      <div>

        <Card fluid className="containerCard">
          <CardBody>
            <Flex><Text>Applaud Card</Text></Flex>
            <Flex space="between">
              <Card selected centered styles={{
                width: '200px',
                borderRadius: '6px',
                marginTop: '5px',
                backgroundColor: "antiquewhite",
                padding: "10px",
                ':hover': {
                  backgroundColor: "antiquewhite",
                },
              }}>
                <div className="detailsBadgeDiv">
                  {this.state.badgeImage && <img src={this.state.badgeImage} />}
                  {this.state.badgeName && <text className="badgeText">{this.state.badgeName}</text>}
                </div>

              </Card>


              {(this.state.teamMembers && this.state.teamMembers.length > 1) ? <div style={{ width: "55%" }}>
                {this.state.teamMembers && <div>
                  <Input value={this.state.searchItem} disabled={(!this.state.multipleSelection && (this.state.selectedEmployeeName.length > 0)) ? true : false} required fluid 
                  // icon={<SearchIcon style={{ height: "15px", width: "15px" }} />} 
                  placeholder="Type a name" onChange={(e) => this.search(e)} />
                  {(this.state.files && (this.state.searchItem)) && <div className='searchList'>
                    {this.state.files.filter((e: any) => e.upn !== this.state.UPN).map((ele: any, i: any) =>
                      <div key={i} className="displayFlex searchBox"  onClick={() => this.selectEmployeeNameFunction(ele)}>
                        {(ele.photo !=="")?<img src={ele.photo} className="profileImage"/>:<img src={baseUrl+"/images/userImage.png"} className="profileImage"/>}
                        <div className='searchResultList'>
                          <Text className="searchResultListEmployeeName"> {ele.displayName} </Text>
                          <Text size="small"> {ele.email} </Text>
                        </div>
                      </div>
                    )}
                  </div>}
                </div>}
                {this.state.selectedEmployeeName && <div>
                  {this.state.selectedEmployeeName.length > 0 && <div style={{ marginTop: "20px" }} className="showSelectedEmployeeDiv">

                    {/* <div style={{ marginBottom: "10px" }}>
                      <Text content={"Employee Name"} size="large" />
                    </div> */}
                    {this.state.selectedEmployeeName.map((e: any, i: any) => {
                      return <Flex column styles={{ marginBottom: "10px" }}>
                        <Flex space="between" styles={{ marginBottom: "5px", alignItems: "center" }} >
                          <div className="displayFlex">
                            {(e.photo !=="")?<img src={e.photo} className="profileImage"/>
                              :<img src={baseUrl+"/images/userImage.png"} className="profileImage"/>
                            }
                          
                          <div className="showSelectedEmployee">
                            <Text content={e.name} size="medium" />
                            <Text content={e.email} size="smallest" />
                          </div>
                          </div>
                          
                          <div className="pointer backButtonMessagingExtention" onClick={() => this.cancelEmployee(i)}>
                            <img src={cancelImage} style={{ height: "12px" }} />
                          </div>

                        </Flex>

                      </Flex>
                    })}

                  </div>}
                </div>}

              </div> : <div style={{ width: "45%" }}>
                {(this.state.selectedEmployeeName && this.state.IsChat) ? <div className="personalChatMember">
                  <Text styles={{ marginBottom: "10px" }} size="medium">To</Text>
                  {this.state.selectedEmployeeName && <Text content={this.state.selectedEmployeeName[0].name} size="medium" />}
                  {this.state.selectedEmployeeName && <Text content={this.state.selectedEmployeeName[0].email} size="medium" styles={{ color: "gray" }} />}
                </div> : null}

              </div>}



            </Flex>

            <Form styles={{ paddingTop: '25px' }}>
              {this.state.formItem.map((e: any) => {
                return <div>
                  {e.dropdown ? (this.state.behaviourInputItem && this.state.viswasBehaviourRequire) && <FormDropdown fluid
                    label={{ content: e.text }}
                    items={this.state.behaviourInputItem}
                    // search={true}
                    className="detalisViswasDropdown"
                    onChange={(event, { value }) => this.check(value, e.key)}
                    placeholder={e.placeholder}
                    value={this.state.behaviours}
                  /> :
                    <FormTextArea label={e.text} value={this.state.reason && this.state.reason} onChange={(event) => this.check(event, e.key)} fluid resize="vertical" placeholder={e.placeholder} className={`textAreaStyles ${this.state.invalidReason && 'invalidReason'}`} />}
                </div>

              })}


            </Form>
          </CardBody>


        </Card>

        <div className="margin20">
          <Flex space="between">
            <div className="backButton pointer backButtonMessagingExtention" onClick={() => this.back()}>
              <img src={backImage} /> <Text size="medium">Back</Text>
            </div>
            <Flex gap="gap.small">
              <Button content="Cancel" onClick={() => microsoftTeams.tasks.submitTask()} />
              <Button disabled={(this.state.selectedEmployeeName.length > 0 && this.state.badgeImage && this.state.badgeName && this.state.reason && (this.state.viswasBehaviourRequire ? this.state.behaviours : true)) ? false : true} primary onClick={() => this.previewBtnFunction()}>Preview</Button>
            </Flex>
          </Flex>
        </div>

      </div>
    );
  }
}



export default Details;

