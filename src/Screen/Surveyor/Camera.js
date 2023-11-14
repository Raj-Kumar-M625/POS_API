import React from "react";
import Webcam from "react-webcam";
import "../../CSS/Views/WebCam.css";
import i18n from "../../i18n/i18n";
import spinner from "../../Assets/Image/spinner.gif"
const videoConstraints = {
  width: 400,
  height: 300,
  facingMode: "user",
};

class Camera extends React.Component {
  constructor(props) {
    super(props);
    this.image = React.createRef();
    this.cam = React.createRef();
    this.capBtn = React.createRef();
    this.arr = []
  }


  setRef = (webcam) => {
    this.webcam = webcam;
  };

  capture = () => {
    debugger;
    this.arr.push(this.webcam.getScreenshot())
    this.props.setimgArray([...this.props.imgArray, this.webcam.getScreenshot()])
  };

  stop = () => {
    let stream = this.webcam.video.srcObject;
    const tracks = stream.getTracks();
    tracks.forEach((track) => {
      track.stop();
    });
    this.webcam.video.srcObject = null;
    this.cam.current.className += " hidden";
    this.props.bool(false);
  };

  retake = () => {
    this.arr = []
    this.props.setimgArray([])
    this.image.current.src = "";
  };

  render() {
    return (
      <div className="web_cam" ref={this.cam} >
        <div className="camContainer">

          <Webcam
            audio={false}
            ref={this.setRef}
            className="video"
            screenshotFormat="image/png"
            videoConstraints={videoConstraints}
            width={400}
            height={300}
          />
        </div>

        <div className="d-flex justify-content-center">
          <button onClick={this.capture} className="btn btn-primary m-1" ref={this.capBtn}>
            {i18n.t("Capture")}
          </button>
          <button className="btn btn-warning m-1" onClick={this.retake}>
            {i18n.t("Retake")}
          </button>

          <button
            onClick={() => {
              this.props.onSubmitHandler()
              console.log("submitted")
            }}
            disabled={this.arr.length == 2 ? false : true}
            className="btn btn-success m-1">
            {this.props.statusCode === 0 ? i18n.t('Save') : <>
              <img src={spinner} width="24px" height="24px" />
            </>}

          </button>
        </div>
      </div>
    );
  }
}

export default Camera;
