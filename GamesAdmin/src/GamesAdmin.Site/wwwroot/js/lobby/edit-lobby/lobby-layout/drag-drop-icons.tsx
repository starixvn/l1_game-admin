import React, { useState } from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";

const grid = 8;

export default class DragDropIconsMemo extends React.Component<any, any> {
  constructor(props) {
    super(props);

    console.log("DragDropIconsMemo constructor ", props);
  }

  render() {
    console.log("DragDropIconsMemo render ");

    const getItemStyle = (isDragging, draggableStyle) => ({
      background: isDragging ? "lightgreen" : "#f3f7fa",
      ...draggableStyle,
    });

    const getListStyle = (isDraggingOver) => ({
      background: isDraggingOver ? "lightblue" : "lightgrey",
      display: "flex",
      padding: grid,
      overflowX: "auto",
    });

    const getGameName = (id) => {
      return this.props.gameIds.find((gameId) => gameId.value == id)?.text;
    };

    return (
      <DragDropContext
        onDragEnd={this.props.onDragEnd}
        onDragUpdate={this.props.onDragUpdate}
      >
        <Droppable droppableId="droppable" direction="horizontal">
          {(provided, snapshot) => (
            <div
              ref={provided.innerRef}
              style={getListStyle(snapshot.isDraggingOver)}
              {...provided.droppableProps}
            >
              {this.props.gameSettings?.map((item, index) => (
                <Draggable
                  key={item.gameId.toString()}
                  draggableId={item.gameId.toString()}
                  index={index}
                >
                  {(provided, snapshot) => (
                    <div
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                      {...provided.dragHandleProps}
                      style={getItemStyle(
                        snapshot.isDragging,
                        provided.draggableProps.style
                      )}
                      className="droppable-icon"
                    >
                      <span>
                        <span>{getGameName(item.gameId)}</span>
                      </span>
                    </div>
                  )}
                </Draggable>
              ))}
              <div>
                {
                  <a
                    className="add-button"
                    href="#"
                    onClick={(e) => this.props.addHandler(e)}
                  >
                    Add Icon
                  </a>
                }
              </div>
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </DragDropContext>
    );
  }
}
