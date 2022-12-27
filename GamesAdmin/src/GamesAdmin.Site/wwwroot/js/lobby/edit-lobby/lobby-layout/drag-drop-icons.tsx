import React from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import { Game, GameSetting } from "../lobby-data-types";

const grid = 8;

type Props = {
  gameSettings: GameSetting[];
  gameIds: Game[];
  addHandler: () => void;
  onDragEnd: (result: any) => void;
  onDragUpdate: (result: any) => void;
  onDragStart: (result: any) => void;
};

export default class DragDropIconsMemo extends React.Component<Props, any> {
  constructor(props: Props) {
    super(props);
  }

  render() {
    const getItemStyle = (isDragging, draggableStyle) => ({
      background: isDragging ? "lightgreen" : "#f3f7fa",
      ...draggableStyle,
    });

    const getListStyle = (isDraggingOver) => ({
      background: isDraggingOver ? "lightblue" : "lightgrey",
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
        onDragStart={this.props.onDragStart}
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
                    onClick={(e) => this.props.addHandler()}
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
