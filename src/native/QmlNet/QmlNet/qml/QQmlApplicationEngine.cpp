#include <QmlNet/qml/QQmlApplicationEngine.h>
#include <QmlNet/types/NetTypeInfo.h>
#include <QmlNet/qml/NetValueType.h>

static int netValueTypeNumber = 0;

#define NETVALUETYPE_CASE(N) \
    case N: NetValueType<N>::init(typeInfo); return qmlRegisterType<NetValueType<N>>(uriString.toUtf8().data(), versionMajor, versionMinor, qmlNameString.toUtf8().data());

extern "C" {

Q_DECL_EXPORT QQmlApplicationEngineContainer* qqmlapplicationengine_create() {
    QQmlApplicationEngineContainer* result = new QQmlApplicationEngineContainer();
    result->qmlEngine = QSharedPointer<QQmlApplicationEngine>(new QQmlApplicationEngine());
    return result;
}

Q_DECL_EXPORT void qqmlapplicationengine_destroy(QQmlApplicationEngineContainer* container) {
    delete container;
}

Q_DECL_EXPORT void qqmlapplicationengine_load(QQmlApplicationEngineContainer* container, LPWSTR path) {
    container->qmlEngine->load(QString::fromUtf16((const char16_t*)path));
}

Q_DECL_EXPORT void qqmlapplicationengine_loadData(QQmlApplicationEngineContainer* container, LPWSTR dataString) {
    container->qmlEngine->loadData(QByteArray::fromStdString(QString::fromUtf16((const char16_t*)dataString).toStdString()));
}

Q_DECL_EXPORT int qqmlapplicationengine_registerType(NetTypeInfoContainer* typeContainer, LPWSTR uri, int versionMajor, int versionMinor, LPWSTR qmlName) {

    QString uriString = QString::fromUtf16((const char16_t*)uri);
    QString qmlNameString = QString::fromUtf16((const char16_t*)qmlName);
    QSharedPointer<NetTypeInfo> typeInfo = typeContainer->netTypeInfo;
    QString fullType = typeInfo->getFullTypeName();

    switch (++netValueTypeNumber) {
        NETVALUETYPE_CASE(1)
        NETVALUETYPE_CASE(2)
        NETVALUETYPE_CASE(3)
        NETVALUETYPE_CASE(4)
        NETVALUETYPE_CASE(5)
        NETVALUETYPE_CASE(6)
        NETVALUETYPE_CASE(7)
        NETVALUETYPE_CASE(8)
        NETVALUETYPE_CASE(9)
        NETVALUETYPE_CASE(10)
        NETVALUETYPE_CASE(11)
        NETVALUETYPE_CASE(12)
        NETVALUETYPE_CASE(13)
        NETVALUETYPE_CASE(14)
        NETVALUETYPE_CASE(15)
        NETVALUETYPE_CASE(16)
        NETVALUETYPE_CASE(17)
        NETVALUETYPE_CASE(18)
        NETVALUETYPE_CASE(19)
        NETVALUETYPE_CASE(20)
        NETVALUETYPE_CASE(21)
        NETVALUETYPE_CASE(22)
        NETVALUETYPE_CASE(23)
        NETVALUETYPE_CASE(24)
        NETVALUETYPE_CASE(25)
        NETVALUETYPE_CASE(26)
        NETVALUETYPE_CASE(27)
        NETVALUETYPE_CASE(28)
        NETVALUETYPE_CASE(29)
        NETVALUETYPE_CASE(30)
        NETVALUETYPE_CASE(31)
        NETVALUETYPE_CASE(32)
        NETVALUETYPE_CASE(33)
        NETVALUETYPE_CASE(34)
        NETVALUETYPE_CASE(35)
        NETVALUETYPE_CASE(36)
        NETVALUETYPE_CASE(37)
        NETVALUETYPE_CASE(38)
        NETVALUETYPE_CASE(39)
        NETVALUETYPE_CASE(40)
        NETVALUETYPE_CASE(41)
        NETVALUETYPE_CASE(42)
        NETVALUETYPE_CASE(43)
        NETVALUETYPE_CASE(44)
        NETVALUETYPE_CASE(45)
        NETVALUETYPE_CASE(46)
        NETVALUETYPE_CASE(47)
        NETVALUETYPE_CASE(48)
        NETVALUETYPE_CASE(49)
        NETVALUETYPE_CASE(50)

        NETVALUETYPE_CASE(51)
        NETVALUETYPE_CASE(52)
        NETVALUETYPE_CASE(53)
        NETVALUETYPE_CASE(54)
        NETVALUETYPE_CASE(55)
        NETVALUETYPE_CASE(56)
        NETVALUETYPE_CASE(57)
        NETVALUETYPE_CASE(58)
        NETVALUETYPE_CASE(59)
        NETVALUETYPE_CASE(60)
        NETVALUETYPE_CASE(61)
        NETVALUETYPE_CASE(62)
        NETVALUETYPE_CASE(63)
        NETVALUETYPE_CASE(64)
        NETVALUETYPE_CASE(65)
        NETVALUETYPE_CASE(66)
        NETVALUETYPE_CASE(67)
        NETVALUETYPE_CASE(68)
        NETVALUETYPE_CASE(69)
        NETVALUETYPE_CASE(70)
        NETVALUETYPE_CASE(71)
        NETVALUETYPE_CASE(72)
        NETVALUETYPE_CASE(73)
        NETVALUETYPE_CASE(74)
        NETVALUETYPE_CASE(75)
        NETVALUETYPE_CASE(76)
        NETVALUETYPE_CASE(77)
        NETVALUETYPE_CASE(78)
        NETVALUETYPE_CASE(79)
        NETVALUETYPE_CASE(80)
        NETVALUETYPE_CASE(81)
        NETVALUETYPE_CASE(82)
        NETVALUETYPE_CASE(83)
        NETVALUETYPE_CASE(84)
        NETVALUETYPE_CASE(85)
        NETVALUETYPE_CASE(86)
        NETVALUETYPE_CASE(87)
        NETVALUETYPE_CASE(88)
        NETVALUETYPE_CASE(89)
        NETVALUETYPE_CASE(90)
        NETVALUETYPE_CASE(91)
        NETVALUETYPE_CASE(92)
        NETVALUETYPE_CASE(93)
        NETVALUETYPE_CASE(94)
        NETVALUETYPE_CASE(95)
        NETVALUETYPE_CASE(96)
        NETVALUETYPE_CASE(97)
        NETVALUETYPE_CASE(98)
        NETVALUETYPE_CASE(99)
        NETVALUETYPE_CASE(100)
    }
    qFatal("Too many registered types");
    return -1;
}

Q_DECL_EXPORT void qqmlapplicationengine_addImportPath(QQmlApplicationEngineContainer* container, LPWSTR path) {
    QString pathString = QString::fromUtf16((const char16_t*)path);
    container->qmlEngine->addImportPath(pathString);
}

}
