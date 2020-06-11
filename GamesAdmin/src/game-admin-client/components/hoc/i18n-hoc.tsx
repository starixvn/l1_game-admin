import React, { ComponentType } from 'react';
import { I18nextProvider } from 'react-i18next';
import { i18n } from 'i18next';
import initialI18N from '../util/i18n';

type WrappedComponentProps = {
  changeLanguage(language: string): void
};
type WrappedComponentState = {
  i18n: i18n
};

export default function withI18N(WrappedComponent: ComponentType<WrappedComponentProps>, LoadingComponent: ComponentType) {
  return class extends React.Component<WrappedComponentProps, WrappedComponentState> {
    constructor(props: WrappedComponentProps) {
      super(props);

      this.state = { i18n: null };
      this.changeLanguage = this.changeLanguage.bind(this);
    }

    changeLanguage(language) {
      if (this.state.i18n) {
        this.state.i18n
          .changeLanguage(language)
          .then(() => this.setState({ i18n: this.state.i18n }));
      }
    }

    componentDidMount() {
      initialI18N()
        .then((i18n) => this.setState({ i18n }));
    }

    render() {
      if (this.state.i18n && this.state.i18n.isInitialized) {
        return (
          <I18nextProvider i18n={this.state.i18n}>
            <WrappedComponent {...this.props} changeLanguage={this.changeLanguage} />
          </I18nextProvider>
        );
      }
      return <LoadingComponent />;
    }
  };
}
