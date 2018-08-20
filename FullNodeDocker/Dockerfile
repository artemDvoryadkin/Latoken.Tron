FROM ubuntu:16.04
LABEL MAINTAINER Artem Dvoryadkin artem.dvoryadkin@gmail.com

ENV DEBIAN_FRONTEND noninteractive
ENV JAVA_HOME       /usr/lib/jvm/java-8-oracle
ENV LANG            en_US.UTF-8
ENV LC_ALL          en_US.UTF-8


RUN apt-get update
RUN apt-get install -y --no-install-recommends locales
RUN locale-gen en_US.UTF-8
RUN apt-get dist-upgrade -y && \
apt-get --purge remove openjdk* && \
echo "oracle-java8-installer shared/accepted-oracle-license-v1-1 select true" | debconf-set-selections && \
echo "deb http://ppa.launchpad.net/webupd8team/java/ubuntu xenial main" > /etc/apt/sources.list.d/webupd8team-java-trusty.list && \
apt-key adv --keyserver keyserver.ubuntu.com --recv-keys EEA14886 && \
apt-get update && \
apt-get install -y --no-install-recommends oracle-java8-installer oracle-java8-set-default   
RUN apt-get -y install wget 
RUN apt-get -y install git
RUN apt-get -y install vim
RUN apt-get clean all 


RUN mkdir -p  /deploy/MainNet  /deploy/TestNet 
RUN cd /deploy/MainNet/
RUN wget   --no-check-certificate  https://raw.githubusercontent.com/tronprotocol/TronDeployment/master/deploy_tron.sh -O  deploy_tron.sh
RUN mv /deploy_tron.sh /deploy/MainNet/
WORKDIR /deploy/MainNet/
RUN bash deploy_tron.sh
WORKDIR /deploy/MainNet/FullNode
ENTRYPOINT ["java", "-jar", "FullNode.jar", "-c", "main_net_config.conf"]

#RUN bash sleep 5d
 #bash deploy_tron.sh


#COPY . /deploy/FullNodeMainNet
#RUN deploy_tron.sh 


#  docker container run -p 18888:18888 -p 50051:50051 -p 8090:8090  -p 10001:10001 -it tronnode9
# bash deploy_tron.sh
# bash deploy_tron.sh
#oot@8b19c13deb12:/deploy/FullNodeSolidityMainNet# ls
#deploy_tron.sh
#root@8b19c13deb12:/deploy/FullNodeSolidityMainNet# bash deploy_tron.sh --app SolidityNode --net mainnet --trust-node 127.0.0.1:50051 --rpc-port 50041