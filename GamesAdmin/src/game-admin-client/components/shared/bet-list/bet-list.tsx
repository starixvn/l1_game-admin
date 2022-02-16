import React from "react";
import Moment from "react-moment";
import style from "./bet-list.module.scss";
import { Row, Col } from "reactstrap";
import { Partners, SystemId } from "../../common/model/BetItem";

const displayTypes = {
  onecolumn: "onecolumn",
  simple: "simple",
  withtime: "withtime",
  withchoice: "withchoice",
};

const renderTitle = (titles, type) => {
  let result = [];
  if (type == displayTypes.withtime) {
    result = [
      <Col key="colTime" className={style.col}>
        {titles[0]}
      </Col>,
      <Col key="colName" className={style.col}>
        {titles[1]}
      </Col>,
      <Col key="colStake" className={style.colamount}>
        {titles[2]}
      </Col>,
    ];
  } else {
    titles.forEach((title, index) => {
      result.push(
        <Col className={style.col} key={index}>
          {title}
        </Col>
      );
    });
  }

  return result;
};

const getDisplayName = (systemId, partnerId, name) => {
  const partnerPrefix = partnerId ? Partners[partnerId] : "";
  const systemSuffix =
    systemId == SystemId.SC ? "" : `[${SystemId[systemId]}] `;

  return `${partnerPrefix}-${name} ${systemSuffix}`;
};

export default function BetList(props) {
  const { type, titles, dataList, isWin } = props;

  return (
    <div className={`${style.betlist} ${isWin ? style.win : ""}`}>
      <div className={style.title}>
        <Row className={style.row}>{renderTitle(titles, type)}</Row>
      </div>
      <div className={style.content}>
        {dataList &&
          dataList.map((data, index) => {
            const betAmount = data.bet.toLocaleString("en-US");

            switch (type) {
              case displayTypes.withtime: {
                return (
                  <Row key={index} className={style.row}>
                    <Col className={style.coltime}>
                      <Moment format="HH:mm:ss">{data.time}</Moment>
                    </Col>
                    <Col className={style.colname}>
                      {getDisplayName(data.system, data.partnerId, data.name)}
                    </Col>
                    <Col className={style.colamount}>{betAmount}</Col>
                  </Row>
                );
              }
              case displayTypes.simple: {
                return (
                  <Row key={index} className={style.rowSimple}>
                    <div className={style.colname}>
                      {getDisplayName(data.system, data.partnerId, data.name)}
                    </div>
                    <div className={style.money}>{betAmount}</div>
                  </Row>
                );
              }
              case displayTypes.withchoice: {
                return (
                  <Row key={index} className={style.rowSimple}>
                    <div className={style.colname}>
                      {getDisplayName(data.system, data.partnerId, data.name)}
                    </div>
                    <div>
                      <span className={style.money}>{betAmount}</span>
                      <span className={style.colname}>@{data.choice}</span>
                    </div>
                  </Row>
                );
              }
              default: {
                return (
                  <Row key={index} className={style.row}>
                    <Col className={style.onecol}>
                      <div>
                        {getDisplayName(data.system, data.partnerId, data.name)}
                      </div>
                      <div className={style.money}>{betAmount}</div>
                    </Col>
                  </Row>
                );
              }
            }
          })}
      </div>
    </div>
  );
}
