public interface Parametrizable
{
	void setParam(string param, string val);

	string getParam(string param);

	ParamList getParameterList();
}
