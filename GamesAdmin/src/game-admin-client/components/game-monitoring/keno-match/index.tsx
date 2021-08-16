import KenoMatch from "./keno-match-base";

export default (gameName: string) => (props) => {
  return <KenoMatch gameName={gameName} {...props} />;
};
