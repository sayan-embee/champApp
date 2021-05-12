import React from 'react';
import { ArrowLeftIcon } from '@fluentui/react-icons-northstar';

import { Header, Flex, Text, Button, Card, CardBody, FlexItem, RadioGroup } from "@fluentui/react-northstar";

import Toggle from 'react-toggle'

import "./../styles.scss"

import { getAppSettingAPI, updateAppSettingAPI } from './../../apis/settingApi'


interface IViswasProps {
  history?: any;
  location?: any
}

interface MyState {
  viswasBehaviourRequire?: any;
  IsGroupSingle?: any;
  IsGroupMultiple?: any;
  IsChannelSingle?: any;
  IsChannelMultiple?: any;
  getData?: any
};

class Setting extends React.Component<IViswasProps, MyState> {
  constructor(props: IViswasProps) {
    super(props);
    this.state = {
    };
}
  componentDidMount() {
    this.getAppSetting()
  }


  getAppSetting() {
    getAppSettingAPI().then((res) => {
      console.log("get app setting", res.data);
      this.setState({
        IsGroupSingle: res.data.isGroupSingle,
        IsGroupMultiple: res.data.isGroupMultiple,
        IsChannelSingle: res.data.isChannelSingle,
        IsChannelMultiple: res.data.isChannelMultiple,
        viswasBehaviourRequire: res.data.isBehaviourRequired,
        getData: true
      })

    })
  }

  viswasBehaviourRequireFunction(e: any) {
    this.setState({
      viswasBehaviourRequire: e.target.checked ? 1 : 0
    })
  }

  channelSetting = (e: any, props: any) => {
    this.setState({
      IsChannelSingle: (props.value === "single") ? 1 : 0,
      IsChannelMultiple: (props.value === "multi") ? 1 : 0
    })
  }

  groupSetting = (e: any, props: any) => {
    this.setState({
      IsGroupSingle: (props.value === "single") ? 1 : 0,
      IsGroupMultiple: (props.value === "multi") ? 1 : 0
    })
  }

  updateSetting() {
    const data = {
      "IsGroupSingle": this.state.IsGroupSingle,
      "IsGroupMultiple": this.state.IsGroupMultiple,
      "IsChannelSingle": this.state.IsChannelSingle,
      "IsChannelMultiple": this.state.IsChannelMultiple,
      "IsBehaviourRequired": this.state.viswasBehaviourRequire
    }
    updateAppSettingAPI(data).then((res) => {
      if (res.data.successFlag === 1) {
        this.getAppSetting()
      }
      console.log(" update setting", res);

    })

  }

  back(){
    this.props.history.push(`/admin_preview`)
}
  render() {
    console.log(this.state);

    return (
      <div>

        <div className="containterBox">
        <div className="displayFlex">
                    <Button onClick={() => this.back()} icon={<ArrowLeftIcon />} text />
          <Header as="h2" content="Settings" style={{ margin: '0', fontWeight: 'lighter' }}></Header>
</div>
          <Card fluid styles={{
            borderRadius: '6px',
            marginTop:"20px",
            backgroundColor: '#f5f5f5',
            ':hover': {
              backgroundColor: '#f5f5f5',
            },
          }}>
            <CardBody>
              <Header as="h4" content="How colleague(s) can be applauded?" ></Header>

              <div className="outerDivToggleRadioGroup">
                <div style={{ marginRight: "15px", display: "flex", flexDirection: "column" }} >
                  <Text content="Group" />
                  <Text content="Channel" styles={{ marginTop: "15px" }} />
                </div>
                {this.state.getData && <div>

                  
                    <RadioGroup
                      defaultCheckedValue={(this.state.IsGroupMultiple === 1) ? "multi" : "single"}
                      onCheckedValueChange={this.groupSetting}
                      items={[
                        { label: 'Single person', value: "single" },
                        { label: 'Multi person', value: "multi" },
                      ]}
                    />
                  
                  <RadioGroup
                    defaultCheckedValue={(this.state.IsChannelSingle === 1) ? "single" : "multi"}
                    onCheckedValueChange={this.channelSetting}
                    items={[
                      { label: 'Single person', value: "single" },
                      { label: 'Multi person', value: "multi" },
                    ]}
                  />
                </div>}


              </div>

              {this.state.getData && <div className="outerDivToggleRadioGroup">
                <Header as="h4" content="Vishvas Behaviours Required?" styles={{ marginRight: "10px" }}></Header>
                <Toggle defaultChecked={(this.state.viswasBehaviourRequire === 1) ? true : false} icons={false} onChange={(e) => this.viswasBehaviourRequireFunction(e)} ></Toggle>
                <Text styles={{ marginLeft: "10px" }}>{this.state.viswasBehaviourRequire ? "Yes" : "No"}</Text>
              </div>}
              <Flex gap="gap.small">
                <FlexItem push>
                  <Button primary content="Update" onClick={() => this.updateSetting()} />
                </FlexItem>
              </Flex>
            </CardBody>
          </Card>


        </div>

      </div>

    );
  }
}



export default Setting;
